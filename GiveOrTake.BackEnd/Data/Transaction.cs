using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.BackEnd.Data
{
    public partial class Transaction
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }

        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        [Required]
        public DateTime OccurenceDate { get; set; }
        public string OtherUserId { get; set; }

        //false Meaning Give, true meaning take
        [Required]
        public bool TransactionType { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}
