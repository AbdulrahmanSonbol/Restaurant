using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.BackgroundJobs
{
    public class ReservationReminderBackgroundJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(10); 

        public ReservationReminderBackgroundJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);

            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                        Console.WriteLine($"[Background Job] Checking upcoming reservations at: {DateTime.UtcNow}");

                        await notificationService.SendReservationRemindersAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Background Job Error]: {ex.Message}");
                }
            }
        }
    }
}
