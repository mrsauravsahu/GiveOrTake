using GiveOrTake.BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class UserController
    {
        private readonly DatabaseService dbService;
        public UserController(DatabaseService dbService)
        { this.dbService = dbService; }

        /// <summary>
        /// User names with mathcing prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>Returns a list of users whose username starts with the specified prefix.</returns>
        [HttpPost]
        public ObjectResult Post([FromQuery]string prefix)
        {
            return new ObjectResult(dbService.GetUsersNameStartingWith(prefix)
                .Select(p => new { Name = p.UserName, Phone = p.Phone }));
        }
    }
}
