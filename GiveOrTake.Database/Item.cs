using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.Database
{
    [Table(nameof(Item))]
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
