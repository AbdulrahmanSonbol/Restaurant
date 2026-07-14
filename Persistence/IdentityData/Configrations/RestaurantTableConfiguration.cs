using Domain.Entities.RestaurantModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.IdentityData.Configrations
{
    public class RestaurantTableConfiguration : IEntityTypeConfiguration<RestaurantTable>
    {
        public void Configure(EntityTypeBuilder<RestaurantTable> builder)
        {
            builder.Property(t => t.RowVersion).IsRowVersion();

            builder.HasIndex(t => new { t.RestaurantId, t.TableNumber }).IsUnique();
        }
    }
}
