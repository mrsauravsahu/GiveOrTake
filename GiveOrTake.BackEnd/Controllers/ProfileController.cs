using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

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

            var userProfile = (from u in dbContext.Users
                               where u.UserId == id
                               select new
                               {
                                   UserId = u.UserId,
                                   Name = u.Name,
                                   FirstName = u.FirstName,
                                   MiddleName = u.MiddleName,
                                   LastName = u.LastName,
                                   Email = u.Email,
                                   Items = u.Items,
                                   Transactions = u.Transactions,
                               }).FirstOrDefault();

            return new ObjectResult(userProfile);
        }
    }
}
