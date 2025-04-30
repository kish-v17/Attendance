using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Attendance.Data;
using Attendance.Models;

namespace Attendance.Services
{
    public class LectureSeederService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public LectureSeederService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Always try to seed on startup in case we missed the scheduled time
            await TrySeedLectureStatusAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    var scheduledTime = DateTime.Today.AddHours(5); // 5:00 AM

                    if (now > scheduledTime)
                    {
                        scheduledTime = scheduledTime.AddDays(1); // Schedule for next day
                    }

                    var delay = scheduledTime - now;

                    await Task.Delay(delay, stoppingToken);

                    await TrySeedLectureStatusAsync();
                }
                catch (Exception ex)
                {
                    // Log the exception (replace with real logger if available)
                    Console.WriteLine($"[LectureSeederService Error] {ex.Message}");
                }
            }
        }

        private async Task TrySeedLectureStatusAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var today = DateTime.Today;
            var currentDayOfWeek = (DaysOfWeek)(((int)today.DayOfWeek == 0) ? 7 : (int)today.DayOfWeek);

            bool isHoliday = _context.HolidayTbl.Any(h =>
                h.HolidayDate.Date == today ||
                (h.IsRecurring && h.HolidayDate.Month == today.Month && h.HolidayDate.Day == today.Day));

            if (isHoliday)
                return;

            bool alreadyExists = _context.LectureStatusTbl.Any(ls => ls.ScheduleDate == today);
            if (alreadyExists)
                return;

            var todayLectures = _context.ScheduleTbl
                .Where(s => s.Day == currentDayOfWeek)
                .ToList();

            if (todayLectures.Any())
            {
                foreach (var lecture in todayLectures)
                {
                    var newLectureStatus = new LectureStatusModel
                    {
                        ScheduleId = lecture.ScheduleId,
                        ScheduleDate = today,
                        FillDate = DateTime.MinValue,
                        Status = LectureStatusEnum.Pending,
                        Remarks = ""
                    };
                    _context.LectureStatusTbl.Add(newLectureStatus);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
