using System.ComponentModel.DataAnnotations;

namespace GiveOrTake.FrontEnd.Shared.Data
{
	public class Person
	{
		[Key]
		public int PersonId { get; set; }
		public string Name { get; set; }
		public uint Age { get; set; }

		public override string ToString() => Name;
	}
}
