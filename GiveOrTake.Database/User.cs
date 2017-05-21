using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.Database
{
    [Table(nameof(User))]
    public class User
    {
        public User()
        {
            Items = new HashSet<Item>();
            Transactions = new HashSet<Transaction>();
        }

        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public ICollection<Item> Items { get; set; }
        public RootAccess RootAccess { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

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
