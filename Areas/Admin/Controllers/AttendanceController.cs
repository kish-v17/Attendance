using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public IActionResult Index(int id, int? studentId, int? subjectId, DateTime? date)
        {
            var query = _context.AttendanceTbl
                .Where(a => a.Schedule.ClassId == id)
            .Include(a => a.Student)
            .Include(a => a.Schedule)
            .Include(a => a.Schedule.Class)
            .Include(a => a.Schedule.Subject)
            .Include(a => a.Schedule.User)
            .AsQueryable();
            if (studentId.HasValue)
            {
                query = query.Where(a => a.StudentId == studentId.Value);
            }

            if (subjectId.HasValue)
            {
                query = query.Where(a => a.Schedule.Subject.SubjectId == subjectId.Value);
            }

            if (date.HasValue)
            {
                query = query.Where(a => a.AttendanceDate == date.Value.Date);
            }

            var attendanceList = query
        .OrderByDescending(a => a.AttendanceDate.Date)
        .ToList();


            ViewBag.Students = _context.StudentTbl.Where(a => a.ClassId == id).ToList();
            ViewBag.Subjects = _context.SubjectTbl.ToList();
            ViewBag.Faculties = _context.UserTbl.ToList();
            ViewBag.Dates = _context.AttendanceTbl
                .Select(a => a.AttendanceDate.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();
            ViewBag.ClassName = _context.ClassTbl
     .Where(a => a.ClassId == id)
     .Include(a => a.Batch)
         .ThenInclude(b => b.Course)
     .Select(a => a.Batch != null && a.Batch.Course != null
         ? $"{a.Batch.Course.CourseShortName} - {a.Batch.Semester} - {a.ClassName}"
         : "N/A")
     .FirstOrDefault();


            return View(attendanceList);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, AttendanceStatus status)
        {
            var attendance = _context.AttendanceTbl.FirstOrDefault(a => a.AttendanceId == id);
            if (attendance == null)
                return NotFound();

            attendance.Status = status;
            _context.SaveChanges();
            TempData["ToastMessage"] = "Status updated successfully!";
            TempData["ToastType"] = "success";
            return Ok();
            //return Json(new { success = true, message = "Status updated successfully!" });

        }

    }
}
