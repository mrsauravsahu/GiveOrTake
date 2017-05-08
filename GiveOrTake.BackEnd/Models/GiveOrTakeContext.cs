using GiveOrTake.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GiveOrTake.BackEnd.Models
{
    public partial class GiveOrTakeContext : DbContext
    {
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<User> User { get; set; }

        public GiveOrTakeContext(DbContextOptions<GiveOrTakeContext> options) : base(options) { }

        public async override void Dispose()
        {
            await this.SaveChangesAsync();
            base.Dispose();
        }

        protected async override void OnModelCreating(ModelBuilder modelBuilder)
        {
            await Database.OpenConnectionAsync();

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserId");

                entity.Property(e => e.ItemId).HasColumnType("int(11)");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UserId).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("item_ibfk_1");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.HasIndex(e => e.ItemId)
                    .HasName("ItemId");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserId");

                entity.Property(e => e.TransactionId).HasColumnType("int(11)");

                entity.Property(e => e.ExpectedReturnDate).HasColumnType("datetime");

                entity.Property(e => e.ItemId).HasColumnType("int(11)");

                entity.Property(e => e.OccurenceDate).HasColumnType("datetime");

                entity.Property(e => e.ShortDescription).HasColumnType("text");

                entity.Property(e => e.TransactionType).HasColumnType("tinyint(1)");

                entity.Property(e => e.UserId).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("transaction_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("transaction_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.UserName)
                    .HasName("UserName")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
            });
        }
    }
}