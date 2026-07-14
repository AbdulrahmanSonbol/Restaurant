using Domain.Entities.RestaurantModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.IdentityData.Configrations
{
    public class RestaurantTagConfiguration : IEntityTypeConfiguration<RestaurantTag>
    {
        public void Configure(EntityTypeBuilder<RestaurantTag> builder)
        {
            builder.HasKey(rt => new { rt.RestaurantId, rt.TagId });

            builder.HasOne(rt => rt.Restaurant)
                .WithMany(r => r.RestaurantTags)
                .HasForeignKey(rt => rt.RestaurantId);

            builder.HasOne(rt => rt.Tag)
                .WithMany(t => t.RestaurantTags)
                .HasForeignKey(rt => rt.TagId);
        }
    }
}
