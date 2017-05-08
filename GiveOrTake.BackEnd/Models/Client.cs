using GiveOrTake.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Models
{
    public class Client
    {
        internal int UserId { get; set; }
        internal string JwtToken { get; set; }
    }
}
