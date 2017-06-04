using System.Threading.Tasks;
using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GiveOrTake.Database;
using Microsoft.AspNetCore.Identity;

namespace GiveOrTake.BackEnd.Controllers
{
	public class RootController : Controller
	{
		private readonly GiveOrTakeContext dbContext;
		private readonly PasswordHasher<User> passwordHasher;

		public RootController(GiveOrTakeContext dbContext,
			PasswordHasher<User> passwordHasher)
		{
			this.dbContext = dbContext;
			this.passwordHasher = passwordHasher;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var rootUsers = await dbContext.Users
				.Include(u => u.RootAccess)
				.Where(u => u.RootAccess != null)
				.OrderBy(u => u.Name)
				.ToListAsync();

			ViewData["RootUsers"] = rootUsers;
			return View();
		}

		[Route("users")]
		[AllowAnonymous]
		[HttpGet]
		public async Task<ObjectResult> GetUsers()
		{
			var users = await dbContext.Users
				.Include(u => u.RootAccess)
				.ToListAsync();
			return new ObjectResult(from u in users
									select new
									{
										UserId = u.UserId,
										Name = u.Name,
										Email = u.Email,
										UserType = u.RootAccess == null ? "Normal" : "Root"
									});
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public IActionResult Login([FromBody]User user)
		{
			if (ModelState.IsValid)
			{
				var users = (from u in dbContext.Users
							 where u.Name == user.Name && u.Email == user.Email
							 select u)
							.Include(u => u.RootAccess)
							.Where(u => u.RootAccess != null)
							.ToList();

				if (users.Count == 0)
					return View();
				else
				{
					if (passwordHasher.VerifyHashedPassword(users[0],
						users[0].RootAccess.Password,
						user.RootAccess.Password) == PasswordVerificationResult.Success)
						return View(nameof(Index));
				}
			}
			return View();
		}
	}
}
