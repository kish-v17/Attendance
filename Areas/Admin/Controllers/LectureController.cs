using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class LectureController : Controller
    {
        private readonly AppDBContext _context;
        public LectureController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var lectures = _context.LectureStatusTbl
                .Include(c => c.Schedule)
                .Include(s => s.Schedule.User)
                .Include(s => s.Schedule.Subject)
                .Include(s => s.Schedule.Class)
                .Include(s => s.Schedule.Class.Batch)
                .Include(s => s.Schedule.Class.Batch.Course)
                .Include(s => s.Schedule.Class.Batch.Course.Department)
                .Select(c => new
                {
                    LectureId = c.Id,
                    LectureDate = c.ScheduleDate.ToShortDateString() +" - "+c.Schedule.Day.ToString(),
                    LectureStatus = c.Status.ToString(),
                    Subject = c.Schedule.Subject.SubjectName,
                    ClassName = $"{c.Schedule.Class.Batch.Course.CourseShortName} -{c.Schedule.Class.Batch.Semester} - {c.Schedule.Class.ClassName}",
                    Duration = c.Schedule.StartTime +" to " + c.Schedule.EndTime,
                    Faculty = c.Schedule.User.Fullname,
                    Department= c.Schedule.Class.Batch.Course.Department.DepartmentShortName
                }).ToList();
            return Json(new { data = lectures });
        }
        public IActionResult Suspend()
        {
            var departments = _context.DepartmentTbl
                .Select(d => new { d.DepartmentId, d.DepartmentName })
                .ToList();
            var slots = _context.ScheduleTbl
                .Select(s => new
                {
                    TimeRange = $"{DateTime.Today.Add(s.StartTime).ToString("hh:mm tt")} - {DateTime.Today.Add(s.EndTime).ToString("hh:mm tt")}"
                })
                .Distinct()
                .ToList();
            ViewBag.Departments = departments;
            ViewBag.Slots = slots;
            return View();
        }
        [HttpPost]
        public IActionResult Suspend(int? DepartmentId, int? ClassId, string TimeSlot, DateTime? Date, string Remarks)
        {
            var lectureQuery = _context.LectureStatusTbl
                .Include(ls => ls.Schedule)
                    .ThenInclude(s => s.Class)
                        .ThenInclude(c => c.Batch)
                            .ThenInclude(b => b.Course)
                                .ThenInclude(course => course.Department)
                .AsQueryable();

            if (DepartmentId.HasValue)
            {
                lectureQuery = lectureQuery.Where(ls => ls.Schedule.Class.Batch.Course.DepartmentId == DepartmentId);
            }

            if (ClassId.HasValue)
            {
                lectureQuery = lectureQuery.Where(ls => ls.Schedule.ClassId == ClassId);
            }

            if (!string.IsNullOrEmpty(TimeSlot) && TimeSlot != "All")
            {
                var parts = TimeSlot.Split('-');
                if (parts.Length == 2 &&
                    DateTime.TryParseExact(parts[0].Trim(), "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDateTime) &&
                    DateTime.TryParseExact(parts[1].Trim(), "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDateTime))
                {
                    var startTime = startDateTime.TimeOfDay;
                    var endTime = endDateTime.TimeOfDay;

                    lectureQuery = lectureQuery.Where(ls =>
                        ls.Schedule.StartTime == startTime &&
                        ls.Schedule.EndTime == endTime);
                }
            }


            if (Date.HasValue)
            {
                lectureQuery = lectureQuery.Where(ls => ls.ScheduleDate.Date == Date.Value.Date);
            }

            var matchingLectures = lectureQuery.ToList();

            if (!matchingLectures.Any())
            {
                TempData["ToastMessage"] = "No matching lectures found to suspend.";
                TempData["ToastType"] = "warning";
                return RedirectToAction("Suspend");
            }

            foreach (var lecture in matchingLectures)
            {
                lecture.Status = LectureStatusEnum.Suspended;
                lecture.Remarks = Remarks;
            }

            _context.SaveChanges();

            TempData["ToastMessage"] = $"{matchingLectures.Count} lecture(s) suspended successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult GetClasses(int departmentId)
        {
            var classes = _context.ClassTbl
                .Where(c => c.Batch.Course.Department.DepartmentId == departmentId)
                 .Include(s => s.Batch)
                .Include(s => s.Batch.Course)
                .Include(s => s.Batch.Course.Department)
                .Select(c => new { c.ClassId, ClassName = $"{c.Batch.Course.CourseShortName} - {c.Batch.Semester} - {c.ClassName}" })
                .ToList();
            classes.Insert(0, new { ClassId = 0, ClassName = "All" });
            return Json(classes);
        }

    }
}
