using GiveOrTake.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GiveOrTake.BackEnd.Models
{
    public partial class GiveOrTakeContext : DbContext
    {
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<NormalUser> NormalUsers { get; set; }
        public virtual DbSet<RootUser> RootUsers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

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

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.UserId).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("item_ibfk_1");
            });

            modelBuilder.Entity<NormalUser>(entity =>
            {
                entity.ToTable("normaluser");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.NormalUser)
                    .HasForeignKey<NormalUser>(d => d.Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("normaluser_ibfk_1");
            });

            modelBuilder.Entity<RootUser>(entity =>
            {
                entity.ToTable("rootuser");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.RootUser)
                    .HasForeignKey<RootUser>(d => d.Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("rootuser_ibfk_1");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.HasIndex(e => e.ItemId)
                    .HasName("ItemId");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ExpectedReturnDate).HasColumnType("datetime");

                entity.Property(e => e.ItemId).HasColumnType("int(11)");

                entity.Property(e => e.OccurenceDate).HasColumnType("datetime");

                entity.Property(e => e.OtherUserId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.TransactionType).HasColumnType("tinyint(1)");

                entity.Property(e => e.UserId).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("transaction_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("transaction_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.MiddleName).HasColumnType("text");
            });
        }
    }
}