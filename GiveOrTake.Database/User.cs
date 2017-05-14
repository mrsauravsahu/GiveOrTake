using System.Collections.Generic;

namespace GiveOrTake.Database
{
    public partial class User
    {
        public User()
        {
            Items = new HashSet<Item>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual NormalUser NormalUser { get; set; }
        public virtual RootUser RootUser { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

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
