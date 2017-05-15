using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class UserController
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
    }
}
