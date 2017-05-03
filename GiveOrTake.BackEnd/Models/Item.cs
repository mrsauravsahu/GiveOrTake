using System;
using System.Collections.Generic;

namespace GiveOrTake.BackEnd.Models
{
    public partial class Item
    {
        public Item()
        {
            Transaction = new HashSet<Transaction>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }
        public virtual User User { get; set; }
    }
}
