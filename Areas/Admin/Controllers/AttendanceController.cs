using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AttendanceController : Controller
    {
        private readonly AppDBContext _context;
        public AttendanceController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var attendance = _context.AttendanceTbl
        .Include(s => s.Student)
        .Include(s => s.Schedule)
            .ThenInclude(c => c.Class)
            .ThenInclude(b => b.Batch)
            .ThenInclude(c => c.Course)
            .ThenInclude(d => d.Department)
        .FirstOrDefault(a => a.AttendanceId == id);
            if (attendance == null)
            {
                return NotFound();
            }
            var model = new AttendanceViewModel
            {
                AttendanceId = attendance.AttendanceId,
                Enrolment = attendance.Student.EnrollmentNumber,
                FullName = attendance.Student.FullName,
                Department = GetShortName(attendance.Schedule.Class.Batch.Course.Department.DepartmentName.ToString()),
                ClassName = attendance.Schedule.Class != null && attendance.Schedule.Class.Batch != null && attendance.Schedule.Class.Batch.Course != null
                    ? $"{attendance.Schedule.Class.Batch.Year}-{GetShortName(attendance.Schedule.Class.Batch.Course.CourseName)} - {attendance.Schedule.Class.Batch.Semester} - {attendance.Schedule.Class.ClassName}"
                    : "N/A",
                Duration = attendance.Schedule.StartTime + " To " + attendance.Schedule.EndTime,
                AttendanceDate = attendance.AttendanceDate.ToString("dd-MM-yyyy"),
                Status = attendance.Status
            };
            return View(model);
        }

        public IActionResult GetAll()
        {
            var attendance = _context.AttendanceTbl.Include(s=>s.Student)
                .Include(s => s.Schedule)
                    .ThenInclude(c=>c.Class)
                    .ThenInclude(b => b.Batch)
                    .ThenInclude(c => c.Course)
                    .ThenInclude(d => d.Department)
                .Select(a => new
                {
                    a.AttendanceId,
                    Enrolment=a.Student.EnrollmentNumber,
                    a.Student.FullName,
                    a.Schedule.Subject.SubjectName,
                    Faculty=a.Schedule.User.Fullname,
                    Department= GetShortName(a.Schedule.Class.Batch.Course.Department.DepartmentName.ToString()),
                    ClassName = a.Schedule.Class != null && a.Schedule.Class.Batch != null && a.Schedule.Class.Batch.Course != null
                    ? $"{a.Schedule.Class.Batch.Year}-{GetShortName(a.Schedule.Class.Batch.Course.CourseName)} - {a.Schedule.Class.Batch.Semester} - {a.Schedule.Class.ClassName}"
                    : "N/A",
                    Date=a.AttendanceDate.ToString("dd-MM-yyyy"),
                    Duration =a.Schedule.StartTime +" To "+a.Schedule.EndTime,
                    Status =a.Status.ToString(),
                }).ToList();

            return Json(new { data = attendance });
        }

        [HttpPost]
        public IActionResult Edit(AttendanceViewModel model)
        {
            var attendance = _context.AttendanceTbl.Find(model.AttendanceId);
            if (attendance == null)
            {
                return NotFound();
            }

            attendance.Status = model.Status; // Update only Status
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Attendance updated successfully!";
            return RedirectToAction("Index"); // Redirect to Attendance List
        }

        private static string GetShortName(string courseName)
        {
            if (string.IsNullOrWhiteSpace(courseName)) return "N/A";

            return new string(courseName.Where(char.IsUpper).ToArray());
        }
    }
}
