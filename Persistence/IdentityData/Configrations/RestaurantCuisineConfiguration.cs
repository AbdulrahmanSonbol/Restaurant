using Domain.Entities.RestaurantModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.IdentityData.Configrations
{
    public class RestaurantCuisineConfiguration : IEntityTypeConfiguration<RestaurantCuisine>
    {
        public void Configure(EntityTypeBuilder<RestaurantCuisine> builder)
        {
            builder.HasKey(rc => new { rc.RestaurantId, rc.CuisineId });

            builder.HasOne(rc => rc.Restaurant)
                .WithMany(r => r.RestaurantCuisines)
                .HasForeignKey(rc => rc.RestaurantId);

            builder.HasOne(rc => rc.Cuisine)
                .WithMany(c => c.RestaurantCuisines)
                .HasForeignKey(rc => rc.CuisineId);
        }
    }
}