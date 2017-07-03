using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveOrTake.Database
{
	public class Transaction
	{
		public Guid TransactionId { get; set; }

		[MaxLength(64)]
		[Required]
		public string Name { get; set; }

		[Required]
		public TransactionType TransactionType { get; set; }

		[MaxLength(255)]
		public string Description { get; set; }

		public DateTime OccurrenceDate { get; set; }
		public DateTime? ExpectedCompletionDate { get; set; }
		public DateTime? CompletionDate { get; set; }

		public Guid ItemId { get; set; }
		public Item Item { get; set; }

		public Guid DeviceId { get; set; }
		public Device Device { get; set; }

		[MaxLength(255)]
		[Required]

		public string UserId { get; set; }
		public User User { get; set; }

		[NotMapped]
		public bool IsComplete => CompletionDate != null;
	}
}
