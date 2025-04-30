using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                }).ToList();
            return Json(new { data = lectures });
        }
        [HttpPost]
        public IActionResult UpdateStatus(int lectureId, LectureStatusEnum newStatus)
        {
            var lecture = _context.LectureStatusTbl.FirstOrDefault(l => l.Id == lectureId);
            if (lecture == null)
            {
                return Json(new { success = false, message = "Lecture not found." });
            }

            lecture.Status = newStatus;
            _context.SaveChanges();

            return Json(new { success = true });
        }

    }
}
