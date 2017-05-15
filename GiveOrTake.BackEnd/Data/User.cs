using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.Database
{
    public partial class User
    {
        public User()
        {
            Items = new HashSet<Item>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual RootAccess RootAccess { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                if (MiddleName == string.Empty)
                    return $"{FirstName} {LastName}";
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }
    }
}
