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

        public IActionResult Index(int id)
        {
            var classInfo = _context.ClassTbl
                .Include(c => c.Batch)
                    .ThenInclude(b => b.Course)
                .FirstOrDefault(c => c.ClassId == id);

            if (classInfo != null)
            {
                ViewData["ClassId"] = id;
                ViewData["ClassName"] = $"{classInfo.Batch.Course.CourseShortName} -  {classInfo.Batch.Semester} - {classInfo.ClassName}";
            }

            return View();
        }
        [HttpGet]
        public IActionResult GetAll(int classId)
        {
            var schedule = _context.ScheduleTbl
                .Include(s => s.Class)
                .ThenInclude(c => c.Batch)
                .ThenInclude(b => b.Course)
                .Include(sub => sub.Subject)
                .Include(f => f.User)
                .Where(s => s.ClassId == classId)  // filter class-wise
                .ToList();

            var transformedSchedules = schedule.Select(s => new
            {
                s.ScheduleId,
                StartTime = s.StartTime.ToString(@"hh\:mm\:ss"),
                EndTime = s.EndTime.ToString(@"hh\:mm\:ss"),
                Faculty = s.User.Fullname,
                Day=s.Day.ToString(),
                Subject = s.Subject.SubjectName,
                ClassName = s.Class != null && s.Class.Batch != null && s.Class.Batch.Course != null
                    ? $"{s.Class.Batch.Year} - {s.Class.Batch.Course.CourseShortName} - {s.Class.Batch.Semester} - {s.Class.ClassName}"
                    : "N/A"
            }).ToList();

            return Json(new { data = transformedSchedules });
        }

        public IActionResult Create(int? classId)
        {
            fetchList();

            var model = new ScheduleModel();
            if (classId != null)
            {
                model.ClassId = classId.Value;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ScheduleModel schedule)
        {
            if (ModelState.IsValid)
            {
                var conflictExists = _context.ScheduleTbl.Any(s =>(
                    s.ClassId == schedule.ClassId &&
                    s.Day == schedule.Day &&
                    (
                        (schedule.StartTime >= s.StartTime && schedule.StartTime < s.EndTime) || 
                        (schedule.EndTime > s.StartTime && schedule.EndTime <= s.EndTime) ||    
                        (schedule.StartTime <= s.StartTime && schedule.EndTime >= s.EndTime)    
                    )
                ) ||
                    (
                        s.FacultyId == schedule.FacultyId &&
                        s.Day == schedule.Day &&
                        (
                            (schedule.StartTime >= s.StartTime && schedule.StartTime < s.EndTime) ||
                            (schedule.EndTime > s.StartTime && schedule.EndTime <= s.EndTime) ||
                            (schedule.StartTime <= s.StartTime && schedule.EndTime >= s.EndTime)
                        )
                    )
                );

                if (conflictExists)
                {
                    TempData["ToastMessage"] = "Schedule conflict: Another schedule exists for this class on the same day and time.";
                    TempData["ToastType"] = "error";

                    fetchList();
                    return View(schedule);
                }

                _context.ScheduleTbl.Add(schedule);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Schedule created successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index", new { id = schedule.ClassId });
            }

            TempData["ToastMessage"] = "Failed to create schedule. Please check the form.";
            TempData["ToastType"] = "error";

            fetchList();
            return View(schedule);
        }

        public IActionResult Edit(int id)
        {
            var schedule = _context.ScheduleTbl.Find(id);
            if (schedule == null)
            {
                TempData["ToastMessage"] = "Schedule not found.";
                TempData["ToastType"] = "error";
                return NotFound();
            }

            var model = new ScheduleModel
            {
                ScheduleId = schedule.ScheduleId,
                SubjectId = schedule.SubjectId,
                ClassId = schedule.ClassId,
                FacultyId = schedule.FacultyId,
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
                    TempData["ToastMessage"] = "Schedule not found.";
                    TempData["ToastType"] = "error";
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

                TempData["ToastMessage"] = "Schedule updated successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index", new { id = schedule.ClassId });
            }

            TempData["ToastMessage"] = "Failed to update schedule. Please check the form.";
            TempData["ToastType"] = "error";

            fetchList();
            return View(model);
        }

        private void fetchList()
        {
            var classList = _context.ClassTbl
                .Include(c => c.Batch)
                .ThenInclude(b => b.Course)
                .Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.Batch != null && c.Batch.Course != null
                        ? $"{c.Batch.Year} - {c.Batch.Course.CourseShortName} - {c.Batch.Semester} - {c.ClassName}"
                        : "N/A"
                })
                .Distinct()
                .ToList();

            ViewBag.ClassList = classList;

            var subjectList = _context.SubjectTbl
                .Select(c => new SelectListItem
                {
                    Value = c.SubjectId.ToString(),
                    Text = c.SubjectName
                })
                .Distinct()
                .ToList();

            ViewBag.SubjectList = subjectList;

            var userList = _context.UserTbl.Where(c => c.UserId != 1)
                .Select(c => new SelectListItem
                {
                    Value = c.UserId.ToString(),
                    Text = "Pro. " + c.Fullname
                })
                .Distinct()
                .ToList();

            ViewBag.FacultyList = userList;
        }
    }
}
