using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.BackEnd.Data
{
    public partial class Item
    {
        public Item()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string ItemName { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual User User { get; set; }
    }
}
