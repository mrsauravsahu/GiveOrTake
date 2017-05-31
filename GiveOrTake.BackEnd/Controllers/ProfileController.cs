using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using GiveOrTake.Database;

namespace GiveOrTake.BackEnd.Controllers
{
	[Route("api/[controller]")]
	public class ProfileController : Controller
	{
		private readonly GiveOrTakeContext dbContext;

		public ProfileController(GiveOrTakeContext dbContext)
		{ this.dbContext = dbContext; }

		[HttpGet]
		public ObjectResult Get()
		{
			//Read Access Token
			var jwtToken = Request.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
			//Retrive id from Access Token
			var id = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Id;

			var user = (from u in dbContext.Users
						where u.UserId == id
						select u)
							   .Include(u => u.Items)
							   .Include(u => u.Transactions)
							   .FirstOrDefault();

			var result = new
			{
				UserId = user.UserId,
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				LastName = user.LastName,
				Email = user.Email,
				Items = (from i in user.Items
						 select new { ItemId = i.ItemId, Name = i.Name }),
				Transactions = (from t in user.Transactions
								select new
								{
									TransactionId = t.TransactionId,
									Name = t.Name,
									Description = t.Description,
									TransactionType = t.TransactionType,
									OccurrenceDate = t.OccurrenceDate,
									ExpectedReturnDate = t.ExpectedReturnDate,
									Item = new Item { Name = t.Item.Name }
								})
			};

			return new ObjectResult(result);
		}
	}
}
