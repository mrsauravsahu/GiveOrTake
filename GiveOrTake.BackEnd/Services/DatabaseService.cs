using GiveOrTake.BackEnd.Models;
using GiveOrTake.Models;
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
        internal async Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            return await Task.Run<ClaimsIdentity>(
                () =>
                {
                    //Check username, password against DB Context
                    var matchedUser = (from u in dbContext.User
                                       where u.UserName == user.UserName
                                       select u).FirstOrDefault();

                    var isUserValid = (matchedUser != null) ?
                        (matchedUser.Password == user.Password) : false;

                    if (isUserValid)
                    {
                        return new ClaimsIdentity(
                          new GenericIdentity(user.UserName, "Token"),
                          new[] { new Claim(nameof(User), user.UserName) });
                    }

                    // Credentials are invalid, or account doesn't exist
                    return null;
                });
        }

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
