using Attendance.Data;
using Attendance.Models;
using Attendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Attendance.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    [Area("Faculty")]
    public class AttendanceController : Controller
    {
        private readonly AppDBContext _context;

        private readonly WhatsAppService _whatsAppService;

        public AttendanceController(AppDBContext context, WhatsAppService whatsAppService)
        {
            _context = context;
            _whatsAppService = whatsAppService;
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
        public IActionResult Fill(int id, DateTime? date)
        {
            var schedule = _context.ScheduleTbl
                .Include(s => s.Class)
                .Include(s => s.Class.Batch)
                .Include(s => s.Class.Batch.Course)
                .Include(s => s.Subject)
                .FirstOrDefault(s => s.ScheduleId == id);

            if (schedule == null)
            {
                TempData["ToastMessage"] = "Schedule not found.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }

            var attendanceDate = date ?? DateTime.Today;

            var statusExists = _context.LectureStatusTbl
                .Any(ls => ls.ScheduleId == id && ls.ScheduleDate == attendanceDate && ls.Status != LectureStatusEnum.Pending);

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
            ViewBag.AttendanceDate = attendanceDate;
            return View(students);
        }

        [HttpPost]
        public async Task<IActionResult> Fill(int scheduleId, List<int> presentStudentIds, bool sendMessage)
        {
            var classId = _context.ScheduleTbl
                .Where(sc => sc.ScheduleId == scheduleId)
                .Select(sc => sc.ClassId)
                .FirstOrDefault();

            var lectureStatus = _context.LectureStatusTbl
                .FirstOrDefault(l => l.ScheduleId == scheduleId);

            if (lectureStatus == null)
                return NotFound("Lecture status not found.");

            var lectureDate = lectureStatus.ScheduleDate.Date;

            var subjectName = _context.ScheduleTbl
                .Where(sc => sc.ScheduleId == scheduleId)
                .Select(sc => sc.Subject.SubjectName)
                .FirstOrDefault();
            subjectName = subjectName?.Trim();


            var students = _context.StudentTbl
                .Where(s => s.ClassId == classId)
                .ToList();

            if (students == null || !students.Any())
                return NotFound("No students found for this class.");

            var absentees = new List<StudentModel>();

            foreach (var student in students)
            {
                bool isPresent = presentStudentIds.Contains(student.StudentId);

                var attendance = new AttendanceModel
                {
                    ScheduleId = scheduleId,
                    StudentId = student.StudentId,
                    AttendanceDate = lectureDate,
                    Status = isPresent ? AttendanceStatus.Present : AttendanceStatus.Absent
                };

                _context.AttendanceTbl.Add(attendance);

                if (!isPresent && sendMessage)
                {
                    absentees.Add(student);
                }
            }

            lectureStatus.Status = LectureStatusEnum.Filled;
            lectureStatus.FillDate = DateTime.Now.Date;

            _context.SaveChanges();

            if (sendMessage && absentees.Any())
            {
                var whatsappService = HttpContext.RequestServices.GetService<WhatsAppService>();

                foreach (var student in absentees)
                {
                    string message = $"Dear Parent,\n\n" +
     $"We would like to inform you that your child, *{student.FullName}*, was marked absent on *{lectureDate:dd-MMM-yyyy}* for the subject *{subjectName}*.\n\n" +
     "Please take note of this. If there's a valid reason for the absence, do let us know.\n\n" +
     "Warm regards,\nAttendance Department";



                    var parentNumber = student.ParentMobileNo.Trim();
                    if (!parentNumber.StartsWith("+91"))
                    {
                        parentNumber = "+91" + parentNumber;
                    }

                    await whatsappService.SendMessageAsync(parentNumber, message);
                }
            }

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
    }
}
