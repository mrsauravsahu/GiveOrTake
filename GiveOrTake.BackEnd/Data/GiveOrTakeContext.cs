using Microsoft.EntityFrameworkCore;

namespace GiveOrTake.BackEnd.Data
{
    public partial class GiveOrTakeContext : DbContext
    {
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<RootAccess> RootAccess { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public GiveOrTakeContext(DbContextOptions<GiveOrTakeContext> options) : base(options)
        { }

        public async override void Dispose()
        {
            await this.SaveChangesAsync();
            base.Dispose();
        }

        protected async override void OnModelCreating(ModelBuilder modelBuilder)
        { await Database.OpenConnectionAsync(); }
    }
}