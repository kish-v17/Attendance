using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Attendance.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDBContext _db;

        public HomeController(AppDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Fetch data dynamically
            var totalStudents = _db.StudentTbl.Count();
            var totalFaculties = _db.UserTbl.Count()-1;
            var totalClasses = _db.ClassTbl.Count();

            // Get today's attendance count
            var today = DateTime.Today;
            var todayAttendance = _db.AttendanceTbl.Count(a => a.AttendanceDate.Date == today);

            // Fetch recent attendance logs (latest 5)
            var recentLogs = _db.AttendanceTbl
                .OrderByDescending(a => a.AttendanceDate)
                .Take(5)
                .Select(a => new
                {
                    StudentName = a.Student.FullName,
                    Branch = a.Student.Class.Batch.Course.Department.DepartmentName,
                    ClassName = a.Student.Class.ClassName,
                    Status = a.Status.ToString(),
                    Time = a.AttendanceDate.ToShortTimeString()
                })
                .ToList();

            // Fetch upcoming classes for today
            var upcomingClasses = _db.ScheduleTbl
               .Where(s => s.Day == (DaysOfWeek)today.DayOfWeek)
               .OrderBy(s => s.StartTime)
               .Take(5)
               .Select(s => new
               {
                   Course = s.Subject.SubjectName,
                   Time = s.StartTime.ToString(@"hh\:mm tt")
               })
               .ToList();

            var model = new
            {
                TotalStudents = totalStudents,
                TotalFaculties = totalFaculties,
                TotalClasses = totalClasses,
                TodayAttendance = todayAttendance,
                RecentLogs = recentLogs,
                UpcomingClasses = upcomingClasses
            };


            return View(model);
        }
    }
}
