using System.Collections.Generic;

namespace GiveOrTake.Models
{
    public partial class User
    {
        public User()
        {
            Items = new HashSet<Item>();
            Transactions = new HashSet<Transaction>();
        }

        public int UserId { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
