using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Areas.Admin.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDBContext _context;

        public CourseController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var courses = _context.CourseTbl.Select(d => new
            {
                d.CourseId,
                d.CourseName,
                d.DepartmentId
            }).ToList();

            return Json(new { data = courses });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CourseModel course)
        {
            if (ModelState.IsValid)
            {
                _context.CourseTbl.Add(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        public IActionResult Edit(int id)
        {
            var course = _context.CourseTbl.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [HttpPost]
        public IActionResult Edit(CourseModel course)
        {
            if (ModelState.IsValid)
            {
                _context.CourseTbl.Update(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }
    }
}
