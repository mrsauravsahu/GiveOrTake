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
					.Include(p => p.Items)
					.Include(p => p.Transactions)
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

		[HttpPost]
		[Route("delete-transaction")]
		public async Task<IActionResult> DeleteTransaction([FromQuery] string id)
		{
			var jwtToken = Request.Headers[AuthHeaderKey].FirstOrDefault().Split(' ')[1];
			var userId = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Id;

			//TODO: Check Parsing string to int on transactionId
			var transactionId = int.Parse(id);


			var transaction = (from t in dbContext.Transactions
							   where t.TransactionId == transactionId && t.UserId == userId
							   select t).FirstOrDefault();

			if (transaction == null)
				return new NotFoundResult();
			else
			{
				dbContext.Transactions.Remove(transaction);
				await dbContext.SaveChangesAsync();
				return new OkResult();
			}
		}

		[HttpPost]
		[Route("add-edit-transaction")]
		public async Task<IActionResult> AddEditTransaction([FromBody] Transaction transaction)
		{
			try
			{
				var user = GetUser(Request.Headers[AuthHeaderKey].First());
				transaction.UserId = user.UserId;
				transaction.User = user;
				if (transaction.Item.ItemId == -1)
				{
					//New Item
					transaction.Item = new Item
					{
						Name = transaction.Item.Name,
						UserId = user.UserId,
						User = user
					};
				}
				if (transaction.TransactionId != -1)
				{
					var tr = (from t in user.Transactions
							  where t.TransactionId == transaction.TransactionId
							  select t).FirstOrDefault();
					user.Transactions.Remove(tr);
				}
				user.Transactions.Add(transaction);
				await dbContext.SaveChangesAsync();
				return new OkResult();
			}
			catch { }
			return new NotFoundResult();
		}

		[HttpGet]
		[Route("items")]
		public IActionResult GetItems()
		{
			var user = GetUser(Request.Headers[AuthHeaderKey].First());

			return new ObjectResult(from i in user.Items
									select new
									{
										Name = i.Name,
										ItemId = i.ItemId
									});
		}
	}
}
