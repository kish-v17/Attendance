using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var courses = _context.CourseTbl.Include("Department")
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseName,
                    c.DepartmentId,
                    DepartmentName = c.Department.DepartmentName // Fetch Department Name directly
                }).ToList();

            return Json(new { data = courses });
        }

        public IActionResult Create()
        {
            var viewModel = new CourseModel
            {
                Departments = _context.DepartmentTbl.ToList() // Fetching departments from DB
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CourseModel model)
        {
            if (ModelState.IsValid)
            {
                var course = new CourseModel
                {
                    CourseName = model.CourseName,
                    DepartmentId = model.DepartmentId
                };

                _context.CourseTbl.Add(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // If validation fails, reload the department list
            model.Departments = _context.DepartmentTbl.ToList();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var course = _context.CourseTbl.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new CourseModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                DepartmentId = course.DepartmentId,
                Departments = _context.DepartmentTbl.ToList() // Fetch departments from DB
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CourseModel model)
        {
            if (ModelState.IsValid)
            {
                var course = _context.CourseTbl.Find(model.CourseId);
                if (course == null)
                {
                    return NotFound();
                }

                course.CourseName = model.CourseName;
                course.DepartmentId = model.DepartmentId;

                _context.CourseTbl.Update(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // If validation fails, reload the department list
            model.Departments = _context.DepartmentTbl.ToList();
            return View(model);
        }

    }
}
