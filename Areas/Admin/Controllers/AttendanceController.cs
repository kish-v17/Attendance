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

        public IActionResult Index(int id,int? studentId, int? subjectId, DateTime? date)
        {
            var query = _context.AttendanceTbl
                .Where(a => a.Schedule.ClassId == id)
            .Include(a => a.Student)
            .Include(a => a.Schedule)
            .Include(a => a.Schedule.Class)
            .Include(a=>a.Schedule.Subject)
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

            
            ViewBag.Students = _context.StudentTbl.Where(a=>a.ClassId==id).ToList();
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

            return Ok();
        }

        //public IActionResult Edit(int id)
        //{
        //    var attendance = _context.AttendanceTbl
        //.Include(s => s.Student)
        //.Include(s => s.Schedule)
        //    .ThenInclude(c => c.Class)
        //    .ThenInclude(b => b.Batch)
        //    .ThenInclude(c => c.Course)
        //    .ThenInclude(d => d.Department)
        //.FirstOrDefault(a => a.AttendanceId == id);
        //    if (attendance == null)
        //    {
        //        return NotFound();
        //    }
        //    var model = new AttendanceViewModel
        //    {
        //        AttendanceId = attendance.AttendanceId,
        //        Enrolment = attendance.Student.EnrollmentNumber,
        //        FullName = attendance.Student.FullName,
        //        Department = attendance.Schedule.Class.Batch.Course.Department.DepartmentShortName.ToString(),
        //        ClassName = attendance.Schedule.Class != null && attendance.Schedule.Class.Batch != null && attendance.Schedule.Class.Batch.Course != null
        //            ? $"{attendance.Schedule.Class.Batch.Year}-{attendance.Schedule.Class.Batch.Course.CourseShortName} - {attendance.Schedule.Class.Batch.Semester} - {attendance.Schedule.Class.ClassName}"
        //            : "N/A",
        //        Duration = attendance.Schedule.StartTime + " To " + attendance.Schedule.EndTime,
        //        AttendanceDate = attendance.AttendanceDate.ToString("dd-MM-yyyy"),
        //        Status = attendance.Status
        //    };
        //    return View(model);
        //}

        //public IActionResult GetAll(int classId)
        //{
        //    var attendance = _context.AttendanceTbl
        //        .Where(s => s.Schedule.ClassId == classId)
        //        .Include(s=>s.Student)
        //        .Include(s => s.Schedule)
        //            .ThenInclude(c=>c.Class)
        //            .ThenInclude(b => b.Batch)
        //            .ThenInclude(c => c.Course)
        //            .ThenInclude(d => d.Department)
        //        .Select(a => new
        //        {
        //            a.AttendanceId,
        //            Enrolment=a.Student.EnrollmentNumber,
        //            a.Student.FullName,
        //            a.Schedule.Subject.SubjectName,
        //            Faculty=a.Schedule.User.Fullname,
        //            Department= a.Schedule.Class.Batch.Course.Department.DepartmentShortName.ToString(),
        //            ClassName = a.Schedule.Class != null && a.Schedule.Class.Batch != null && a.Schedule.Class.Batch.Course != null
        //            ? $"{a.Schedule.Class.Batch.Year}-{a.Schedule.Class.Batch.Course.CourseShortName} - {a.Schedule.Class.Batch.Semester} - {a.Schedule.Class.ClassName}"
        //            : "N/A",
        //            Date=a.AttendanceDate.ToString("dd-MM-yyyy"),
        //            Duration = $"{a.Schedule.StartTime:hh\\:mm} To {a.Schedule.EndTime:hh\\:mm}",
        //            Status =a.Status.ToString(),
        //        }).ToList();

        //    return Json(new { data = attendance });
        //}

        //[HttpPost]
        //public IActionResult Edit(AttendanceViewModel model)
        //{
        //    var attendance = _context.AttendanceTbl.Find(model.AttendanceId);
        //    if (attendance == null)
        //    {
        //        TempData["ToastMessage"] = "Attendance not found.";
        //        TempData["ToastType"] = "error";
        //        return NotFound();
        //    }

        //    attendance.Status = model.Status; 
        //    _context.SaveChanges();

        //    TempData["ToastMessage"] = "Attendance updated successfully!";
        //    TempData["ToastType"] = "success";
        //    return RedirectToAction("Index");
        //} //[HttpPost]
        //public IActionResult Edit(AttendanceViewModel model)
        //{
        //    var attendance = _context.AttendanceTbl.Find(model.AttendanceId);
        //    if (attendance == null)
        //    {
        //        TempData["ToastMessage"] = "Attendance not found.";
        //        TempData["ToastType"] = "error";
        //        return NotFound();
        //    }

        //    attendance.Status = model.Status; 
        //    _context.SaveChanges();

        //    TempData["ToastMessage"] = "Attendance updated successfully!";
        //    TempData["ToastType"] = "success";
        //    return RedirectToAction("Index");
        //}
    }
}
