using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using GiveOrTake.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GiveOrTake.BackEnd.Controllers
{
	[Route("api/[controller]")]
	public class UserController : Controller
	{
		private const string AuthHeaderKey = "Authorization";
		private readonly GiveOrTakeContext dbContext;

		public UserController(GiveOrTakeContext dbContext)
		{ this.dbContext = dbContext; }

		private User GetUser(string token)
		{
			var userId = new JwtSecurityTokenHandler()
				.ReadJwtToken(token.Split(' ')[1])
				.Id;

			return (from u in dbContext.Users
					where u.UserId == userId
					select u)
					.Include(p => p.Item)
					.Include(p => p.Transaction)
					.FirstOrDefault();
		}

		/// <summary>
		/// User names with mathcing prefix.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <returns>Returns a list of users whose username starts with the specified prefix.</returns>
		[HttpGet]
		public IActionResult Get([FromQuery]string prefix)
		{
			if (string.IsNullOrWhiteSpace(prefix))
				return new BadRequestResult();

			prefix = prefix.ToUpper();
			return new ObjectResult(
				from u in dbContext.Users
				where u.Name.ToUpper().StartsWith(prefix)
				select new { Name = u.Name });
		}

		[HttpGet]
		[Route("items")]
		public IActionResult GetItems()
		{
			var user = GetUser(Request.Headers[AuthHeaderKey].First());

			return new ObjectResult(from i in user.Item
									select new
									{
										Name = i.Name,
										ItemId = i.ItemId
									});
		}
	}
}
