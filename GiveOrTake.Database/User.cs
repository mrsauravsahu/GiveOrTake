using GiveOrTake.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GiveOrTake.Database
{
	public class User
	{
		public User()
		{
			Item = new HashSet<Item>();
			Transaction = new HashSet<Transaction>();
			Device = new HashSet<Device>();
		}

		[MaxLength(255)]
		public string UserId { get; set; }

		[MaxLength(64)]
		[Required]
		public string FirstName { get; set; }

		[MaxLength(64)]
		public string MiddleName { get; set; }

		[MaxLength(64)]
		[Required]
		public string LastName { get; set; }

		[MaxLength(64)]
		[EmailAddress]
		[Required]
		public string Email { get; set; }

		[NotMapped]
		public string Name
		{
			get
			{
				if (MiddleName == string.Empty)
					return $"{FirstName} {LastName}";
				return $"{FirstName} {MiddleName} {LastName}";
			}
		}

		public RootAccess RootAccess { get; set; }
		public HashSet<Item> Item { get; set; }
		public HashSet<Transaction> Transaction { get; set; }
		public HashSet<Device> Device { get; set; }
	}
}
