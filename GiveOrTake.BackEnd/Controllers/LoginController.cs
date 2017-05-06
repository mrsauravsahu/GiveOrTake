using GiveOrTake.BackEnd.Helpers;
using GiveOrTake.BackEnd.Services;
using GiveOrTake.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LoginController
    {
        private readonly JwtIssuerOptions jwtOptions;
        private readonly ILogger logger;
        private readonly JsonSerializerSettings serializerSettings;
        private readonly DatabaseService dbService;

        public LoginController(
            IOptions<JwtIssuerOptions> jwtOptions,
            ILoggerFactory loggerFactory,
            DatabaseService dbService)
        {
            this.jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(this.jwtOptions);
            logger = loggerFactory.CreateLogger<LoginController>();
            serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            this.dbService = dbService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var identity = await dbService.GetClaimsIdentity(user);
            if (identity == null)
            {
                logger.LogInformation($"Invalid username ({user.UserName}) or password ({user.Password})");
                return new BadRequestResult();
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
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
            // Serialize and return the response
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

            if (options.JtiGenerator == null)
            { throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator)); }
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}
