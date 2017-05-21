using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using GiveOrTake.BackEnd.Helpers;
using Microsoft.Extensions.Options;
using GiveOrTake.Database;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController
    {
        private GiveOrTakeContext dbContext;
        private readonly PasswordHasher<User> passwordHasher;
        private readonly RootUserOptions rootUserOptions;

        public RegisterController(GiveOrTakeContext dbContext,
            IOptions<RootUserOptions> rootUserOptions,
            PasswordHasher<User> passwordHasher)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
            this.rootUserOptions = rootUserOptions.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] dynamic user)
        {
            string firstName = user.FirstName.ToString();
            string middleName = user.MiddleName.ToString();
            string lastName = user.LastName.ToString();
            string password = user.Password.ToString();
            string email = user.Email.ToString();
            string masterPassword = user.MasterPassword.ToString();

            if (masterPassword != rootUserOptions.MasterPassword)
                return new BadRequestResult();

            if (string.IsNullOrWhiteSpace(firstName)
                || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(password)
                || string.IsNullOrWhiteSpace(email))
                return new BadRequestResult();
            else
            {
                var guid = Guid.NewGuid().ToString();
                var newUser = new User
                {
                    UserId = guid,
                    FirstName = firstName,
                    MiddleName = middleName != null ? middleName.Trim() : string.Empty,
                    LastName = lastName,
                    Email = email,
                    Items = new List<Item>(),
                    Transactions = new List<Transaction>()
                };
                newUser.RootAccess = new RootAccess
                { Password = passwordHasher.HashPassword(newUser, password) };

                var result = (await dbContext.Users.AddAsync(newUser)).Entity;
                await dbContext.SaveChangesAsync();
                return new CreatedAtRouteResult(this, new
                {
                    Name = result.Name,
                    Email = result.Email
                });
            }
        }
    }
}
