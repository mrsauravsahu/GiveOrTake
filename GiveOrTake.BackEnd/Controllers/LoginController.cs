using GiveOrTake.BackEnd.Helpers;
using GiveOrTake.BackEnd.Data;
using GiveOrTake.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GiveOrTake.BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LoginController
    {
        private readonly JwtIssuerOptions jwtOptions;
        private readonly ILogger logger;
        private readonly JsonSerializerSettings serializerSettings;
        private readonly GiveOrTakeContext dbContext;
        private readonly PasswordHasher<User> passwordHasher;

        public LoginController(
            IOptions<JwtIssuerOptions> jwtOptions,
            ILoggerFactory loggerFactory,
            GiveOrTakeContext dbContext,
            PasswordHasher<User> passwordHasher)
        {
            this.jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(this.jwtOptions);
            logger = loggerFactory.CreateLogger<LoginController>();
            serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] dynamic user)
        {
            string UserName = user.Name;
            string Password = user.Password;

            //Check username, password against DB Context
            var matchedUser = (from u in dbContext.Users
                               where u.Name == UserName
                               select u).FirstOrDefault();

            if (matchedUser == null)
            {
                logger.LogInformation($"Non existent user ({UserName}: {Password})");
                return new BadRequestResult();
            }

            var root = (from p in dbContext.RootAccess
                        where p.Id == matchedUser.Id
                        select p).FirstOrDefault();

            if (root == null)
            {
                logger.LogInformation($"User without Root Access tried to log in. ({UserName}: {Password})");
                return new BadRequestResult();
            }

            bool validCredentials = (this.passwordHasher.VerifyHashedPassword(
                matchedUser,
                root.Password,
                Password) == PasswordVerificationResult.Success);

            ClaimsIdentity identity;
            if (validCredentials)
            {
                identity = new ClaimsIdentity(
                  new GenericIdentity(UserName, "Token"),
                  new[] { new Claim(nameof(User), UserName) });
            }
            else
            {
                // Credentials are invalid, or account doesn't exist
                logger.LogInformation($"Invalid password ({Password}) for ({UserName})");
                return new BadRequestResult();
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, matchedUser.Name),
                new Claim(JwtRegisteredClaimNames.Jti, matchedUser.Id),
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
            var response = new
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds
            };
            return new OkObjectResult(response);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.ValidFor <= TimeSpan.Zero)
            { throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor)); }

            if (options.SigningCredentials == null)
            { throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials)); }
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}