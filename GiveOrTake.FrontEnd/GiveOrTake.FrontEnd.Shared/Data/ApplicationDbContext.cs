using GiveOrTake.Database;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GiveOrTake.FrontEnd.Shared.Data
{
	class ApplicationDbContext : DbContext
	{
		private string localFolderPath;

		public DbSet<User> Users { get; set; }
		public DbSet<Device> Devices { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<RootAccess> RootAccess { get; set; }

		public ApplicationDbContext(string databasePath)
		{ this.localFolderPath = databasePath; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Filename={Path.Combine(localFolderPath, nameof(ApplicationDbContext))}");
		}
	}
}
