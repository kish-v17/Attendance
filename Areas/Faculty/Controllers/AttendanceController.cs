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
       private int fetchFacultyId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var facultyIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            return Convert.ToInt32(facultyIdClaim.Value);
        }
        public IActionResult Index()
        {
            int facultyId = fetchFacultyId();
            if (facultyId == null)
            {
                TempData["ToastMessage"] = "Unauthorized access.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Login", "Account");
            }

            var today = DateTime.Today;
            var currentDay = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), today.DayOfWeek.ToString());

            var todayLectures = _context.ScheduleTbl
                .Include(s => s.Subject)
                .Include(s => s.Class)
                    .ThenInclude(c => c.Batch)
                        .ThenInclude(b => b.Course)
                .Where(s => s.FacultyId == facultyId && s.Day == currentDay)
                .OrderBy(s => s.StartTime)
                .ToList();

            var lectureStatuses = _context.LectureStatusTbl
                .Where(ls => ls.ScheduleDate.Date == today)
                .ToList();

            var lecturesWithStatus = todayLectures.Select(l => new
            {
                Lecture = l,
                AttendanceStatus = lectureStatuses.FirstOrDefault(ls => ls.ScheduleId == l.ScheduleId)?.Status ?? LectureStatusEnum.Pending,
                FullClassName = $"{l.Class.Batch.Course.CourseShortName} -{l.Class.Batch.Semester} - {l.Class.ClassName}"
            }).ToList<dynamic>();

            return View(lecturesWithStatus);
        }

        public IActionResult Fill(int id)
        {
            var schedule = _context.ScheduleTbl
                .Include(s => s.Class)
                .Include(s=>s.Class.Batch)
                .Include(s=>s.Class.Batch.Course)
                .Include(s => s.Subject)
                .FirstOrDefault(s => s.ScheduleId == id);

            if (schedule == null)
            {
                TempData["ToastMessage"] = "Schedule not found.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }

            var today = DateTime.Today;
            var statusExists = _context.LectureStatusTbl
                .Any(ls => ls.ScheduleId == id && ls.ScheduleDate == today && ls.Status != LectureStatusEnum.Pending);

            if (statusExists)
            {
                TempData["ToastMessage"] = "Attendance already filled or lecture not in pending state.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }

            var students = _context.StudentTbl
                .Where(st => st.ClassId == schedule.ClassId)
                .ToList();

            ViewBag.Schedule = schedule;
            return View(students);
        }

        [HttpPost]
        public IActionResult Fill(int scheduleId, List<int> presentStudentIds)
        {
            var students = _context.StudentTbl
                .Where(s => s.ClassId == _context.ScheduleTbl
                    .Where(sc => sc.ScheduleId == scheduleId)
                    .Select(sc => sc.ClassId)
                    .FirstOrDefault())
                .ToList();

            if (students == null || !students.Any())
            {
                return NotFound();
            }

            foreach (var student in students)
            {
                var attendance = new AttendanceModel
                {
                    ScheduleId = scheduleId,
                    StudentId = student.StudentId,
                    AttendanceDate = DateTime.Now.Date,
                    Status = presentStudentIds.Contains(student.StudentId) ? AttendanceStatus.Present : AttendanceStatus.Absent
                };

                _context.AttendanceTbl.Add(attendance);
            }

            var lectureStatus = _context.LectureStatusTbl.FirstOrDefault(l => l.ScheduleId == scheduleId);
            if (lectureStatus != null)
            {
                lectureStatus.Status = LectureStatusEnum.Filled;
                lectureStatus.FillDate = DateTime.Now.Date;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Attendance");
        }

        public IActionResult PendingAttendance(DateTime? date)
        {
            int facultyId = fetchFacultyId();
            if (facultyId == 0)
            {
                TempData["ToastMessage"] = "Unauthorized access.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Login", "Account");
            }

            var query = _context.LectureStatusTbl
                .Where(ls => ls.Status == LectureStatusEnum.Pending &&
                             ls.Schedule.FacultyId == facultyId)
                .Include(s => s.Schedule)
                .Include(s => s.Schedule.Class)
                .Include(s => s.Schedule.Class.Batch)
                .Include(s => s.Schedule.Class.Batch.Course)
                .Select(ls => new
                {
                    ScheduleId = ls.ScheduleId,
                    SubjectName = ls.Schedule.Subject.SubjectName,
                    FullClassName = ls.Schedule.Class.Batch.Course.CourseShortName + " - " +
                                    ls.Schedule.Class.Batch.Semester + " - " + ls.Schedule.Class.ClassName,
                    ScheduleDate = ls.ScheduleDate,
                    StartTime = ls.Schedule.StartTime,
                    EndTime = ls.Schedule.EndTime,
                    //AttendanceStatus = ls.Status
                });

            if (date.HasValue)
            {
                query = query.Where(ls => ls.ScheduleDate == date.Value.Date);
            }

            var result = query.ToList();
            return View(result);
        }



        //public IActionResult SuspendLecture(int id)
        //{
        //    var schedule = _context.ScheduleTbl
        //        .Include(s => s.Class)
        //        .FirstOrDefault(s => s.ScheduleId == id);

        //    if (schedule == null)
        //    {
        //        TempData["ToastMessage"] = "Schedule not found.";
        //        TempData["ToastType"] = "error";
        //        return RedirectToAction("Index");
        //    }

        //    var today = DateTime.Today;

        //    var statusExists = _context.LectureStatusTbl
        //        .Any(ls => ls.ScheduleId == id && ls.Date == today && ls.Status != LectureStatusEnum.Pending);

        //    if (statusExists)
        //    {
        //        TempData["ToastMessage"] = "Lecture already processed.";
        //        TempData["ToastType"] = "error";
        //        return RedirectToAction("Index");
        //    }

        //    var lectureStatus = new LectureStatusModel
        //    {
        //        ScheduleId = id,
        //        Date = today,
        //        Status = LectureStatusEnum.Suspended
        //    };

        //    _context.LectureStatusTbl.Add(lectureStatus);
        //    _context.SaveChanges();

        //    TempData["ToastMessage"] = "Lecture suspended successfully.";
        //    TempData["ToastType"] = "success";
        //    return RedirectToAction("Index");
        //}
    }
}
