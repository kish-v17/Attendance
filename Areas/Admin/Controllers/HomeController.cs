using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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
            var totalStudents = _db.StudentTbl.Count();
            var totalFaculties = _db.UserTbl.Count()-1;
            var totalClasses = _db.ClassTbl.Count();

            var today = DateTime.Today;
            var todayEnum = (DaysOfWeek)((int)today.DayOfWeek);
            var todayAttendance = _db.AttendanceTbl.Count(a => a.AttendanceDate.Date == today);

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

            var upcomingClasses = _db.ScheduleTbl
               .Where(s => s.Day == todayEnum)
               .Include(s=>s.Subject)
               .OrderBy(s => s.StartTime)
               .Take(5)
               .AsEnumerable()
               .Select(s => new
               {
                   Course = s.Subject.SubjectName,
                   Time = s.StartTime != null ? s.StartTime.ToString("hh\\:mm") : "N/A"
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
