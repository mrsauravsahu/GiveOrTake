using GiveOrTake.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GiveOrTake.BackEnd.Data
{
	public partial class GiveOrTakeContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Device> Devices { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<RootAccess> RootAccess { get; set; }

		public GiveOrTakeContext(DbContextOptions<GiveOrTakeContext> options) : base(options)
		{ Database.EnsureCreated(); }

		protected async override void OnModelCreating(ModelBuilder modelBuilder)
		{
			await Database.OpenConnectionAsync();
		}
		public async override void Dispose()
		{
			await this.SaveChangesAsync();
			base.Dispose();
		}
	}
}