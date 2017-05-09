using GiveOrTake.BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

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
            var jwtToken = Request.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
            var id = int.Parse(new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Id);

            return new ObjectResult((from u in dbContext.User
                                     where u.UserId == id
                                     select new
                                     {
                                         UserId = u.UserId,
                                         UserName = u.UserName,
                                         Phone = u.Phone,
                                         Item = u.Item,
                                         Transaction = u.Transaction
                                     }).FirstOrDefault());
        }
    }
}
