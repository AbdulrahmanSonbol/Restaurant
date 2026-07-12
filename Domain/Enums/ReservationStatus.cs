using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum ReservationStatus
    {
        Pending = 1,
        Confirmed = 2,
        Seated = 3,
        Completed = 4,
        Cancelled = 5,
        NoShow = 6
    }
}
