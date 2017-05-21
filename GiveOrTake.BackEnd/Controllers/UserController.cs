using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly GiveOrTakeContext dbContext;
        public UserController(GiveOrTakeContext dbContext)
        { this.dbContext = dbContext; }

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
        public IActionResult DeleteTransaction([FromQuery] string id)
        {
            var jwtToken = Request.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
            var userId = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Id;

            //TODO: Check Parsing string to int on transactionId

            var transaction = (from t in dbContext.Transactions
                               where t.TransactionId == int.Parse(id) && t.UserId == userId
                               select t).FirstOrDefault();

            if (transaction == null)
                return new NotFoundResult();
            else
            {
                dbContext.Transactions.Remove(transaction);
                return new OkResult();
            }
        }
    }
}
