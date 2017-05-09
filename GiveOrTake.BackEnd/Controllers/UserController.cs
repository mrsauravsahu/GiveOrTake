using GiveOrTake.BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
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
        public ObjectResult Get([FromQuery]string prefix)
        {
            prefix = prefix.ToUpper();
            return new ObjectResult(
                from u in dbContext.User
                where u.UserName.ToUpper().StartsWith(prefix)
                select new { Name = u.UserName, Phone = u.Phone });
        }
    }
}
