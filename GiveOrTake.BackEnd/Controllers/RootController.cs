using System.Threading.Tasks;
using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class RootController
    {
        private readonly GiveOrTakeContext dbContext;

        public RootController(GiveOrTakeContext dbContext)
        { this.dbContext = dbContext; }

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
    }
}
