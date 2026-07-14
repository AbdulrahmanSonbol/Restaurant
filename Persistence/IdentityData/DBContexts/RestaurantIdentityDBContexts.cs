using Domain.Entities.IdentityModule;
using Domain.Entities.RestaurantModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.IdentityData.DBContexts
{
    public class RestaurantIdentityDBContexts
        : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public RestaurantIdentityDBContexts(
            DbContextOptions<RestaurantIdentityDBContexts> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Identity

            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            #endregion

            #region RestaurantCuisine

            builder.Entity<RestaurantCuisine>()
                .HasKey(rc => new { rc.RestaurantId, rc.CuisineId });

            builder.Entity<RestaurantCuisine>()
                .HasOne(rc => rc.Restaurant)
                .WithMany(r => r.RestaurantCuisines)
                .HasForeignKey(rc => rc.RestaurantId);

            builder.Entity<RestaurantCuisine>()
                .HasOne(rc => rc.Cuisine)
                .WithMany(c => c.RestaurantCuisines)
                .HasForeignKey(rc => rc.CuisineId);

            #endregion

            #region RestaurantTag

            builder.Entity<RestaurantTag>()
                .HasKey(rt => new { rt.RestaurantId, rt.TagId });

            builder.Entity<RestaurantTag>()
                .HasOne(rt => rt.Restaurant)
                .WithMany(r => r.RestaurantTags)
                .HasForeignKey(rt => rt.RestaurantId);

            builder.Entity<RestaurantTag>()
                .HasOne(rt => rt.Tag)
                .WithMany(t => t.RestaurantTags)
                .HasForeignKey(rt => rt.TagId);

            #endregion
            #region MenuItem

            builder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);

            builder.Entity<MenuItem>()
                .Property(m => m.RowVersion)
                .IsRowVersion();

            builder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent Multiple Cascade Paths
            builder.Entity<MenuItem>()
                .HasOne(m => m.Restaurant)
                .WithMany(r => r.MenuItems)
                .HasForeignKey(m => m.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
            #region Payment

            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            builder.Entity<Payment>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            builder.Entity<Payment>()
                .HasOne(p => p.Reservation)
                .WithOne(r => r.Payment)
                .HasForeignKey<Payment>(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Payment>()
                .HasOne(p => p.VerifiedByUser)
                .WithMany()
                .HasForeignKey(p => p.VerifiedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Review

            builder.Entity<Review>()
                .Property(r => r.RowVersion)
                .IsRowVersion();

            builder.Entity<Review>()
                .HasOne(r => r.Reservation)
                .WithOne(res => res.Review)
                .HasForeignKey<Review>(r => r.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Restaurant)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
            #region Reservation

            builder.Entity<Reservation>()
                .Property(r => r.DepositAmount)
                .HasPrecision(18, 2);

            builder.Entity<Reservation>()
                .Property(r => r.RowVersion)
                .IsRowVersion();

            builder.Entity<Reservation>()
                .HasOne(r => r.Restaurant)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reservation>()
                .HasOne(r => r.RestaurantTable)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.RestaurantTableId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
            #region Favorite

            builder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Favorite>()
                .HasOne(f => f.Restaurant)
                .WithMany(r => r.Favorites)
                .HasForeignKey(f => f.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.RestaurantId })
                .IsUnique();

            #endregion
        }

        #region DbSets

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantImage> RestaurantImages { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<RestaurantCuisine> RestaurantCuisines { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RestaurantTag> RestaurantTags { get; set; }
        public DbSet<RestaurantWorkingHour> RestaurantWorkingHours { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }

        #endregion
    }
}