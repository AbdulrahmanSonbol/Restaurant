using Domain.Entities.RestaurantModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Specifications
{
    public class UpcomingReservationsSpecification : BaseSpecifications<Reservation, int>
    {
        public UpcomingReservationsSpecification()
               : base(r =>
                   r.ReservationDate == DateOnly.FromDateTime(DateTime.UtcNow) && 
                   r.ReservationTime >= TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(2).AddMinutes(-5)) &&
                   r.ReservationTime <= TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(2).AddMinutes(5)) &&
                   !r.IsNotificationSent &&
                   r.FcmToken != null &&
                   r.FcmToken != "")
        {
            AddInclude(r => r.User);
        }
    }
}
