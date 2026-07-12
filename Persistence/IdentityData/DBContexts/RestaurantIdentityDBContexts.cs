using Domain.Entities.IdentitMyodule;
using Domain.Entities.RestaurantModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.IdentityData.DBContexts
{
    public class RestaurantIdentityDBContexts : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public RestaurantIdentityDBContexts(DbContextOptions<RestaurantIdentityDBContexts> options)
         : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        }


        public DbSet<Restaurant> Restaurants { get; set; }

    }
}
