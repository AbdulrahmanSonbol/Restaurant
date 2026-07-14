using Domain.Entities.RestaurantModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.IdentityData.Configrations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(r => r.DepositAmount).HasPrecision(18, 2);
            builder.Property(r => r.RowVersion).IsRowVersion();

            builder.HasOne(r => r.Restaurant)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.RestaurantTable)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.RestaurantTableId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
