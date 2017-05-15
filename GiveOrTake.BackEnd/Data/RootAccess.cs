using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.Database
{
    public partial class RootAccess
    {
        [Key, ForeignKey(nameof(User))]
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }

        public virtual User User { get; set; }
    }
}
