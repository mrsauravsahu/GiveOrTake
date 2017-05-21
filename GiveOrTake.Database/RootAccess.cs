using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.Database
{
    public class RootAccess
    {
        public string UserId { get; set; }
        public string Password { get; set; }

        public virtual User User { get; set; }
    }
}
