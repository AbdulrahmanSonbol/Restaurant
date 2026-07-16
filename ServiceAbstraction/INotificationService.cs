using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceAbstraction
{
    public interface INotificationService
    {
        Task SendReservationRemindersAsync(CancellationToken cancellationToken);
    }
}
