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
							   .Include(u => u.Item)
							   .Include(u => u.Transaction)
							   .FirstOrDefault();

			var result = new
			{
				UserId = user.UserId,
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				LastName = user.LastName,
				Email = user.Email,
				Items = user.Item.Select(i => new { ItemId = i.ItemId, Name = i.Name }),
				Transactions = user.Transaction.Select(t => new
				{
					TransactionId = t.TransactionId,
					Name = t.Name,
					Description = t.Description,
					TransactionType = t.TransactionType,
					OccurrenceDate = t.OccurrenceDate,
					ExpectedReturnDate = t.ExpectedCompletionDate,
					Item = new Item { Name = t.Item.Name }
				})
			};

			Debug.WriteLine(JsonConvert.SerializeObject(result));
			return new ObjectResult(result);
		}
	}
}
