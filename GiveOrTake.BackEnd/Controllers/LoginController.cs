using GiveOrTake.BackEnd.Helpers;
using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using GiveOrTake.Database;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace GiveOrTake.BackEnd.Controllers
{
	[Route("api/[controller]")]
	public class LoginController : Controller
	{
		private readonly JwtIssuerOptions jwtOptions;
		private readonly ILogger logger;
		private readonly JsonSerializerSettings serializerSettings;
		private readonly GiveOrTakeContext dbContext;
		private readonly PasswordHasher<User> passwordHasher;
		private readonly LoginHelper loginHelper;

		public LoginController(
			IOptions<JwtIssuerOptions> jwtOptions,
			ILoggerFactory loggerFactory,
			GiveOrTakeContext dbContext,
			PasswordHasher<User> passwordHasher,
			LoginHelper loginHelper)
		{
			this.jwtOptions = jwtOptions.Value;
			logger = loggerFactory.CreateLogger<LoginController>();
			serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
			this.dbContext = dbContext;
			this.passwordHasher = passwordHasher;
			this.loginHelper = loginHelper;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] dynamic user)
		{
			string userName = user.Name;
			string email = user.Email;
			string password = user.Password;

			var (token, success) = await this.loginHelper.Login(userName, email, password);

			if (success)
				return new OkObjectResult(token);
			return new BadRequestResult();
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("IsLoggedIn")]
		public IActionResult IsLoggedIn()
		{
			var header = Request.Headers["Authorization"].FirstOrDefault();
			if (header != null)
			{
				try
				{
					var token = header.Split(' ')[1];
					var id = new JwtSecurityTokenHandler().ReadJwtToken(token).Id;
					if (dbContext.Users.Any(u => u.UserId == id))
						return new OkResult();
				}
				catch (Exception) { }
			}
			return new BadRequestResult();
		}
	}
}