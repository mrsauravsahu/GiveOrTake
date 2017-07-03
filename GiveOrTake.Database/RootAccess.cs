using System.ComponentModel.DataAnnotations;

namespace GiveOrTake.Database
{
	public class RootAccess
	{
		[Key]
		[MaxLength(255)]
		[Required]
		public string UserId { get; set; }

		[MaxLength(255)]
		[Required]
		public string Password { get; set; }

		public User User { get; set; }
	}
}
