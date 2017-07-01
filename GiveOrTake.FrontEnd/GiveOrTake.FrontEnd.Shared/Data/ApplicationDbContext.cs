using Microsoft.EntityFrameworkCore;

namespace GiveOrTake.FrontEnd.Shared.Data
{
	class ApplicationDbContext : DbContext
	{
		public DbSet<Person> People { get; set; }
		private string databasePath;

		public ApplicationDbContext(string databasePath)
		{
			this.databasePath = databasePath;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Filename={databasePath}");
		}
	}
}
