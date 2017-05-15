using GiveOrTake.BackEnd.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using GiveOrTake.BackEnd.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;

namespace GiveOrTake.BackEnd.Helpers
{
    public class LoginHelper
    {
        private readonly JwtIssuerOptions jwtOptions;
        private readonly ILogger<LoginHelper> logger;
        private readonly JsonSerializerSettings serializerSettings;
        private readonly GiveOrTakeContext dbContext;
        private readonly PasswordHasher<User> passwordHasher;

        public LoginHelper(IOptions<JwtIssuerOptions> jwtOptions,
            ILoggerFactory loggerFactory,
            GiveOrTakeContext dbContext,
            PasswordHasher<User> passwordHasher)
        {
            this.jwtOptions = jwtOptions.Value;
            logger = loggerFactory.CreateLogger<LoginHelper>();
            serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }


        #region FacebookLogin
        private async Task<Dictionary<string, string>> facebookDetails(string accessToken)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://graph.facebook.com/v2.5/me?fields=id,name,email,first_name,middle_name,last_name&access_token={accessToken}");

                return await Task.Run(async () =>
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                });
            }
        }

        public async Task<User> FacebookLogin(string accessToken)
        {
            var data = await facebookDetails(accessToken);
            return new User
            {
                Id = data["id"],
                FirstName = data["first_name"],
                MiddleName = data.ContainsKey("middle_name") ? data["middle_name"] : string.Empty,
                LastName = data["last_name"],
                Email = data["email"],
                Items = new List<Item>(),
                Transactions = new List<Transaction>(),
                RootAccess = null
            };
        }

        public async Task<string> FacebookId(string accessToken)
        { return ((await facebookDetails(accessToken))["id"]); }
        #endregion

        #region RootLogin
        public async Task<Token> GenerateAuthToken(User user)
        {
            return await Task.Run(() =>
            {
                var identity = new ClaimsIdentity(
                    new GenericIdentity(user.Name, "Token"),
                    new[] { new Claim(nameof(User), user.Name) });

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.Iat,
                    ToUnixEpochDate(jwtOptions.IssuedAt).ToString(),
                    ClaimValueTypes.Integer64),
                    identity.FindFirst(nameof(User))
                };

                // Create the JWT security token and encode it.
                var jwt = new JwtSecurityToken(
                   issuer: jwtOptions.Issuer,
                   audience: jwtOptions.Audience,
                   claims: claims,
                   notBefore: jwtOptions.NotBefore,
                   expires: jwtOptions.Expiration,
                   signingCredentials: jwtOptions.SigningCredentials);

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return new Token
                {
                    AccessToken = encodedJwt,
                    ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds
                };
            });
        }

        public async Task<(Token token, bool success)> Login(string userName, string email, string password)
        {
            //Check username, password against DB Context
            var matchedUser = (from u in dbContext.Users
                               where u.Name == userName && u.Email == email
                               select u).FirstOrDefault();

            if (matchedUser == null) { return (null, false); }

            var root = (from p in dbContext.RootAccess
                        where p.Id == matchedUser.Id
                        select p).FirstOrDefault();

            if (root == null) { return (null, false); }

            bool validCredentials = (this.passwordHasher.VerifyHashedPassword(
                matchedUser,
                root.Password,
                password) == PasswordVerificationResult.Success);

            if (validCredentials) return (await GenerateAuthToken(matchedUser), true);
            else
            {
                // Credentials are invalid, or account doesn't exist
                return (null, false);
            }
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
        #endregion
    }
}
