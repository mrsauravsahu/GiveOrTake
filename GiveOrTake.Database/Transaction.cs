using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.Database
{
    [Table(nameof(Transaction))]
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string Description { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public DateTime OccurrenceDate { get; set; }
        public int TransactionType { get; set; }
        public string UserId { get; set; }

        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}
