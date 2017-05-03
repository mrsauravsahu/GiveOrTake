using GiveOrTake.BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class UserController
    {
        private readonly GiveOrTakeContext db;
        public UserController(GiveOrTakeContext context) { this.db = context; }

        [HttpGet]
        public ObjectResult Get()
        {
            return new ObjectResult(
                from u in db.User
                select new { u.UserName, u.Phone }
                );
        }
    }
}
