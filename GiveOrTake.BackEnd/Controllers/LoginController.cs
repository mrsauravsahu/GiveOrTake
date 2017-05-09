using GiveOrTake.BackEnd.Helpers;
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
        public IActionResult Post([FromBody] User user)
        {
            //Check username, password against DB Context
            var matchedUser = (from u in dbContext.User
                               where u.UserName == user.UserName
                               select u).FirstOrDefault();

            var isUserValid = (matchedUser != null) ?
                (this.passwordHasher.VerifyHashedPassword(
                    matchedUser,
                    matchedUser.Password,
                    user.Password) == PasswordVerificationResult.Success) : false;

            ClaimsIdentity identity;
            if (isUserValid)
            {
                identity = new ClaimsIdentity(
                  new GenericIdentity(user.UserName, "Token"),
                  new[] { new Claim(nameof(User), user.UserName) });
            }
            else
            {
                // Credentials are invalid, or account doesn't exist
                logger.LogInformation($"Invalid username ({user.UserName}) or password ({user.Password})");
                return new BadRequestResult();
            }

            var claims = new[]
                        {
                new Claim(JwtRegisteredClaimNames.Sub, matchedUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, matchedUser.UserId.ToString()),
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
                access_token = encodedJwt,
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
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