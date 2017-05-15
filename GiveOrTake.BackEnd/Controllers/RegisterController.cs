using GiveOrTake.BackEnd.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

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
        public async Task<IActionResult> Post([FromBody] dynamic user)
        {
            string firstName = user.FirstName.ToString();
            string middleName = user.MiddleName.ToString();
            string lastName = user.LastName.ToString();
            string password = user.Password.ToString();
            string email = user.Email.ToString();

            if (string.IsNullOrWhiteSpace(firstName)
                || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(password)
                || string.IsNullOrWhiteSpace(email))
                return new BadRequestResult();
            else
            {
                var guid = new Guid().ToString();
                var newUser = new User
                {
                    Id = guid,
                    FirstName = firstName,
                    MiddleName = (middleName ?? string.Empty),
                    LastName = lastName,
                    Email = email,
                    Items = new List<Item>(),
                    Transactions = new List<Transaction>(),
                    RootAccess = new RootAccess
                    {
                        Id = guid,
                        Password = string.Empty
                    }
                };

                var result = (await dbContext.Users.AddAsync(newUser)).Entity;

                result.RootAccess.Password = passwordHasher.HashPassword(newUser, password);
                result.RootAccess.User = result;

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
