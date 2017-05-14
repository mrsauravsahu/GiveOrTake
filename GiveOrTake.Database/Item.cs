using System.Collections.Generic;

namespace GiveOrTake.Database
{
    public partial class Item
    {
        public Item()
        {
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string ItemName { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }
        public virtual User User { get; set; }
    }
}
