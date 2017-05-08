using System;

namespace GiveOrTake.Database
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public int ItemId { get; set; }
        public DateTime OccurenceDate { get; set; }
        public string ShortDescription { get; set; }
        public bool TransactionType { get; set; }
        public int UserId { get; set; }

        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}
