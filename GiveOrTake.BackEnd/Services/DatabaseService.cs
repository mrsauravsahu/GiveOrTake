using GiveOrTake.BackEnd.Models;
using GiveOrTake.Database;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Services
{
    public class DatabaseService
    {
        private readonly GiveOrTakeContext dbContext;
        public DatabaseService(GiveOrTakeContext dbContext)
        { this.dbContext = dbContext; }

        

        //Get Claims for the user based on UserName and Password provided
        

        internal IEnumerable<User> GetUsersNameStartingWith(string prefix)
        {
            if (prefix.Length < 2) return (new User[] { }).ToList();

            prefix = prefix.ToUpper();

            return (from u in dbContext.User
                    where u.UserName.ToUpper().StartsWith(prefix)
                    select u);
        }
    }
}
