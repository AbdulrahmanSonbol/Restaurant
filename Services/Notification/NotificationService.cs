using Domain.Entities.RestaurantModule;
using FirebaseAdmin.Messaging;
using ServiceAbstraction;
using Services.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SendReservationRemindersAsync(CancellationToken cancellationToken)
        {
            var reservationRepo = _unitOfWork.GetRepository<Reservation, int>();

            var spec = new UpcomingReservationsSpecification();

            var upcomingReservations = await reservationRepo.ListWithSpecAsync(spec, cancellationToken);

            if (!upcomingReservations.Any()) return;

            foreach (var reservation in upcomingReservations)
            {
                try
                {
                    var clientName = reservation.User != null ? reservation.User.UserName : "Guest";

                    await SendPushNotificationAsync(
                        reservation.FcmToken!,
                        "Reservation Reminder",
                        $"Hello {clientName}, your reservation is in 2 hours. We are waiting for you!"
                    );

                    reservation.IsNotificationSent = true;
                    reservationRepo.Update(reservation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Firebase Error] Failed for reservation {reservation.Id}: {ex.Message}");
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task SendPushNotificationAsync(string deviceToken, string title, string body)
        {
            var message = new Message()
            {
                Fid = deviceToken, 
                Notification = new FirebaseAdmin.Messaging.Notification() { Title = title, Body = body }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
    