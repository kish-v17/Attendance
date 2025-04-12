using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ScheduleController : Controller
    {
        private readonly AppDBContext _context;

        public ScheduleController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var schedule = _context.ScheduleTbl
                .Include(s => s.Class)
                .ThenInclude(c => c.Batch)
                .ThenInclude(b => b.Course)
                .Include(sub => sub.Subject)
                .Include(f => f.User)
                .ToList();

            var transformedSchedules = schedule.Select(s => new
            {
                s.ScheduleId,
                Day = s.Day.ToString(),
                StartTime = s.StartTime.ToString(@"hh\:mm\:ss"), 
                EndTime = s.EndTime.ToString(@"hh\:mm\:ss"),
                Faculty = s.User.Fullname,
                Subject = s.Subject.SubjectName,
                ClassName = s.Class != null && s.Class.Batch != null && s.Class.Batch.Course != null
                    ? $"{s.Class.Batch.Year} - {GetShortName(s.Class.Batch.Course.CourseName)} - {s.Class.Batch.Semester} - {s.Class.ClassName}"
                    : "N/A"
            }).ToList();

            return Json(new { data = transformedSchedules });
        }

        public IActionResult Create()
        {
            fetchList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ScheduleModel schedule)
        {
            if (ModelState.IsValid)
            {
                _context.ScheduleTbl.Add(schedule);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            fetchList();
            return View(schedule);
        }

        public IActionResult Edit(int id)
        {
            var schedule = _context.ScheduleTbl.Find(id);
            if (schedule == null)
            {
                return NotFound();
            }
            var model = new ScheduleModel
            {
                ScheduleId = schedule.ScheduleId,
                SubjectId = schedule.SubjectId,
                ClassId = schedule.ClassId,
                FacultyId= schedule.FacultyId,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Day = schedule.Day,
            };
            fetchList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(ScheduleModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = _context.ScheduleTbl.Find(model.ScheduleId);
                if (schedule == null)
                {
                    return NotFound();
                }
                schedule.FacultyId = model.FacultyId;
                schedule.SubjectId = model.SubjectId;
                schedule.ClassId = model.ClassId;
                schedule.StartTime = model.StartTime;
                schedule.EndTime = model.EndTime;
                schedule.Day = model.Day;
                _context.ScheduleTbl.Update(schedule);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            fetchList();
            return View(model);
        }
        private static string GetShortName(string courseName)
        {
            if (string.IsNullOrWhiteSpace(courseName)) return "N/A";

            return new string(courseName.Where(char.IsUpper).ToArray());
        }
        private void fetchList()
        {
            var classList = _context.ClassTbl
            .Include(c => c.Batch)
            .ThenInclude(b => b.Course)
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.ClassId.ToString(),
                Text = c.Batch != null && c.Batch.Course != null
                    ? $"{c.Batch.Year} - {GetShortName(c.Batch.Course.CourseName)} - {c.Batch.Semester} - {c.ClassName}"
                    : "N/A"
            })
            .Distinct()
            .ToList();

            ViewBag.ClassList = classList;

            var subjectList = _context.SubjectTbl
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.SubjectId.ToString(),
                Text = c.SubjectName
            })
            .Distinct()
            .ToList();

            ViewBag.SubjectList = subjectList;

            var userList = _context.UserTbl.Where(c => c.UserId != 1)
           .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
           {
               Value = c.UserId.ToString(),
               Text = "Pro. " + c.Fullname.ToString()
           })
           .Distinct()
           .ToList();

            ViewBag.FacultyList = userList;
        }
    }
}
