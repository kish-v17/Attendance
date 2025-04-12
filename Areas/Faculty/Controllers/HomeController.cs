﻿using Attendance.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Attendance.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    [Area("Faculty")]
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var facultyIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (facultyIdClaim == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect if not authenticated
            }

            int facultyId = Convert.ToInt32(facultyIdClaim.Value);

            var timeSlots = new[]
            {
                new { Start = "08:00", End = "09:45", SlotName = "1st Slot" },
                new { Start = "09:45", End = "10:00", SlotName = "Tea Break" },
                new { Start = "10:00", End = "11:40", SlotName = "2nd Slot" },
                new { Start = "11:40", End = "12:30", SlotName = "Lunch Break" },
                new { Start = "12:30", End = "14:10", SlotName = "3rd Slot" }
            };

            var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            // Fetch faculty schedule from DB
            var schedule = _context.ScheduleTbl
                .Where(s => s.FacultyId == facultyId)
                .Include(s => s.Subject)
                .Include(c => c.Class)
                    .ThenInclude(b => b.Batch)
                    .ThenInclude(c => c.Course)
                .ToList() // Materialize the query
                .Where(s => daysOfWeek.Contains(s.Day.ToString()))
                .Select(s => new
                {
                    Day = s.Day.ToString(), // Normalize Day to string
                    s.StartTime,
                    s.EndTime,
                    ClassName = s.Class.ClassName,
                    Semester = s.Class.Batch.Semester,
                    CourseName = GetCapitalLetters(s.Class.Batch.Course.CourseName),
                    SubjectName = GetCapitalLetters(s.Subject.SubjectName)
                })
                .ToList();

            // Prepare timetable dictionary
            var timetable = new Dictionary<string, Dictionary<string, string>>();

            foreach (var day in daysOfWeek)
            {
                timetable[day] = new Dictionary<string, string>();

                foreach (var slot in timeSlots)
                {
                    var slotStart = TimeSpan.Parse(slot.Start);
                    var slotEnd = TimeSpan.Parse(slot.End);

                    var existingSchedule = schedule.FirstOrDefault(s =>
                        s.Day.Equals(day, StringComparison.OrdinalIgnoreCase) &&
                        s.StartTime.Equals(slotStart) &&
                        s.EndTime.Equals(slotEnd)
                    );

                    timetable[day][slot.Start + " - " + slot.End] = existingSchedule != null
    ? $"{existingSchedule.Semester} - {existingSchedule.CourseName} - {existingSchedule.ClassName}<br/><strong>{existingSchedule.SubjectName}</strong>"
    : (slot.SlotName.Contains("Break") ? $"<strong>{slot.SlotName}</strong>" : "-");

                }
            }

            ViewBag.Timetable = timetable;
            return View();
        }

        private string GetCapitalLetters(string input)
        {
            return new string(input.Where(char.IsUpper).ToArray());
        }
    }
}
