using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public long ExpriesIn { get; set; }
    }
}
