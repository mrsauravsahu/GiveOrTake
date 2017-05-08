using GiveOrTake.BackEnd.Models;
using GiveOrTake.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController
    {
        private GiveOrTakeContext dbContext;
        private readonly PasswordHasher<User> passwordHasher;

        public RegisterController(GiveOrTakeContext dbContext,
            PasswordHasher<User> passwordHasher)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
                return new BadRequestResult();
            else
            {
                var newUser = new User
                {
                    UserName = user.UserName,
                    Item = new List<Item>(),
                    Transaction = new List<Transaction>(),
                    Phone = user.Phone
                };
                newUser.Password = passwordHasher.HashPassword(newUser, user.Password);
                await dbContext.User.AddAsync(newUser);
                return new CreatedAtRouteResult(this, new
                {
                    UserName = newUser.UserName,
                    Phone = newUser.Phone
                });
            }
        }
    }
}
