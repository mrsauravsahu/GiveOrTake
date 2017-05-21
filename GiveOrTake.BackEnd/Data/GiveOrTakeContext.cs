using GiveOrTake.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        
        protected async override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.HasIndex(e => e.Name)
                    .HasName("Name_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_Item_User1_idx");

                entity.Property(e => e.ItemId).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Item_User1");
            });

            modelBuilder.Entity<RootAccess>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("fk_RootAccess_User_idx");

                entity.ToTable("rootaccess");

                entity.Property(e => e.UserId).HasColumnType("varchar(255)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.RootAccess)
                    .HasForeignKey<RootAccess>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_RootAccess_User");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_Transaction_User1_idx");

                entity.Property(e => e.TransactionId).HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.ExpectedReturnDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.OccurrenceDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionType).HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Transaction_User1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.UserId).HasColumnType("varchar(255)");

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

            await Database.OpenConnectionAsync();
        }
    }
}