using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiveOrTake.Database
{
	public class Device
	{
		public Device()
		{ Transaction = new HashSet<Transaction>(); }
		public Guid DeviceId { get; set; }

		[MaxLength(64)]
		[Required]
		public string Name { get; set; }

		[MaxLength(255)]
		[Required]

		public string UserId { get; set; }
		[JsonIgnore]
		public User User { get; set; }
		[JsonIgnore]
		public HashSet<Transaction> Transaction { get; set; }
	}
}
