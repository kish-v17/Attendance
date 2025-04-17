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
                Department = attendance.Schedule.Class.Batch.Course.Department.DepartmentShortName.ToString(),
                ClassName = attendance.Schedule.Class != null && attendance.Schedule.Class.Batch != null && attendance.Schedule.Class.Batch.Course != null
                    ? $"{attendance.Schedule.Class.Batch.Year}-{attendance.Schedule.Class.Batch.Course.CourseShortName} - {attendance.Schedule.Class.Batch.Semester} - {attendance.Schedule.Class.ClassName}"
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
                    Department= a.Schedule.Class.Batch.Course.Department.DepartmentShortName.ToString(),
                    ClassName = a.Schedule.Class != null && a.Schedule.Class.Batch != null && a.Schedule.Class.Batch.Course != null
                    ? $"{a.Schedule.Class.Batch.Year}-{a.Schedule.Class.Batch.Course.CourseShortName} - {a.Schedule.Class.Batch.Semester} - {a.Schedule.Class.ClassName}"
                    : "N/A",
                    Date=a.AttendanceDate.ToString("dd-MM-yyyy"),
                    Duration = $"{a.Schedule.StartTime:hh\\:mm} To {a.Schedule.EndTime:hh\\:mm}",
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
                TempData["ToastMessage"] = "Attendance not found.";
                TempData["ToastType"] = "error";
                return NotFound();
            }

            attendance.Status = model.Status; 
            _context.SaveChanges();

            TempData["ToastMessage"] = "Attendance updated successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
