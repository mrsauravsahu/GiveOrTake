using System.Collections.Generic;

namespace GiveOrTake.Database
{
    public partial class User
    {
        public User()
        {
            Item = new HashSet<Item>();
            Transaction = new HashSet<Transaction>();
        }

        public int UserId { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
