using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PantrySaver.Models
{
    public partial class PantrySaverContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public PantrySaverContext()
        {
        }

        public PantrySaverContext(DbContextOptions<PantrySaverContext> options) : base(options)
        {
        }

        public virtual DbSet<UserAddress> UserAddresses { get; set; } = null!;
        public virtual DbSet<PantryOwn> PantryOwns { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Pantry> Pantries { get; set; } = null!;
        public virtual DbSet<PantryItem> PantryItems { get; set; } = null!;
        public virtual DbSet<EmailSupport> EmailSupports { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserAddress>(entity =>
            {
                entity.HasKey(e => e.UserAddressId)
                    .HasName("PK__UserAddressId");

                entity.ToTable("UserAddress");

                entity.Property(e => e.UserAddressId).HasColumnName("UserAddressID");

                entity.Property(e => e.AddressName).HasColumnName("AddressName");

                entity.Property(e => e.Address1).HasColumnName("Address1");

                entity.Property(e => e.Address2).HasColumnName("Address2");

                entity.Property(e => e.Address3).HasColumnName("Address3");

                entity.Property(e => e.City).HasColumnName("City");

                entity.Property(e => e.State).HasColumnName("State");

                entity.Property(e => e.Zipcode).HasColumnName("Zipcode");

                entity.Property(e => e.Country).HasColumnName("Country");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserAddress__UserID");
            });

            builder.Entity<PantryOwn>(entity =>
            {
                entity.ToTable("PantryOwn");

                entity.Property(e => e.PantryOwnId).HasColumnName("PantryOwnID");

                entity.Property(e => e.Role).HasMaxLength(450);

                entity.Property(e => e.PantryId)
                    .HasMaxLength(450)
                    .HasColumnName("PantryID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Pantry)
                    .WithMany(p => p.PantryOwns)
                    .HasForeignKey(d => d.PantryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PantryOwn__Pantr__787EE5A0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PantryOwns)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PantryOwn__UserI__778AC167");
            });

            builder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.BarcodeFormats).HasMaxLength(450);

                entity.Property(e => e.Category).HasMaxLength(450);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(450)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.IsCustom).HasColumnName("isCustom");

                entity.Property(e => e.Manufacturer).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            builder.Entity<Pantry>(entity =>
            {
                entity.ToTable("Pantry");

                entity.Property(e => e.PantryId).HasColumnName("PantryID");

                entity.Property(e => e.Location).HasMaxLength(450);

                entity.Property(e => e.PantryName).HasMaxLength(450);
            });

            builder.Entity<PantryItem>(entity =>
            {
                entity.ToTable("PantryItem");

                entity.Property(e => e.PantryItemId).HasColumnName("PantryItemID");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(450)
                    .HasColumnName("ItemID");

                entity.Property(e => e.PantryId)
                    .HasMaxLength(450)
                    .HasColumnName("PantryID");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.PantryItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PantryIte__ItemI__7D439ABD");

                entity.HasOne(d => d.Pantry)
                    .WithMany(p => p.PantryItems)
                    .HasForeignKey(d => d.PantryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PantryIte__Pantr__7E37BEF6");
            });

            builder.Entity<EmailSupport>(entity =>
            {
                entity.ToTable("EmailSupport");

                entity.HasKey(e => e.EmailSupportId);

                entity.Property(e => e.EmailSupportId)
                    .HasColumnName("EmailSupportID");

                entity.Property(e => e.EmailFrom)
                    .HasMaxLength(450)
                    .HasColumnName("EmailFrom");

                entity.Property(e => e.Content)
                    .HasColumnName("Content");

                entity.Property(e => e.Answer)
                    .HasColumnName("Answer");
            });

            OnModelCreatingPartial(builder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}