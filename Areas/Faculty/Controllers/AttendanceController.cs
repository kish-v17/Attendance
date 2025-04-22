using Attendance.Areas.Admin.Controllers;
using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Attendance.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    [Area("Faculty")]
    public class AttendanceController : Controller
    {
        private readonly AppDBContext _context;

        public AttendanceController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var facultyIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (facultyIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int facultyId = Convert.ToInt32(facultyIdClaim.Value);
            var today = DateTime.Today;
            var currentDay = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), today.DayOfWeek.ToString());

            var todayLectures = _context.ScheduleTbl
                .Include(s => s.Subject)
                .Include(s => s.Class)
                .Where(s => s.FacultyId == facultyId && s.Day == currentDay)
                .OrderBy(s => s.StartTime) // 🔥 Sort here
                .ToList();

            var attendedScheduleIds = _context.AttendanceTbl
                .Where(a => a.AttendanceDate == today && a.Schedule.FacultyId == facultyId)
                .Select(a => a.ScheduleId)
                .ToList();

            var lecturesWithStatus = todayLectures.Select(l => new
            {
                Lecture = l,
                AttendanceStatus = attendedScheduleIds.Contains(l.ScheduleId) ? "Filled" : "Pending"
            }).ToList();

            return View(lecturesWithStatus);
        }


    }
}
