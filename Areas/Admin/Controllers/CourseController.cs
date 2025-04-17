using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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
                    c.CourseShortName,
                    DepartmentName = c.Department.DepartmentShortName
                }).ToList();

            return Json(new { data = courses });
        }

        public IActionResult Create()
        {
            var viewModel = new CourseModel
            {
                Departments = _context.DepartmentTbl.ToList()
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
                    CourseShortName = model.CourseShortName,
                    DepartmentId = model.DepartmentId
                };

                _context.CourseTbl.Add(course);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Course created successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to create course. Please check the details.";
            TempData["ToastType"] = "error";

            model.Departments = _context.DepartmentTbl.ToList();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var course = _context.CourseTbl.Find(id);
            if (course == null)
            {
                TempData["ToastMessage"] = "Course not found!";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }

            var viewModel = new CourseModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseShortName = course.CourseShortName,
                DepartmentId = course.DepartmentId,
                Departments = _context.DepartmentTbl.ToList()
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
                    TempData["ToastMessage"] = "Course not found!";
                    TempData["ToastType"] = "error";
                    return RedirectToAction("Index");
                }

                course.CourseName = model.CourseName;
                course.CourseShortName = model.CourseShortName;
                course.DepartmentId = model.DepartmentId;

                _context.CourseTbl.Update(course);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Course updated successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to update course. Please check the details.";
            TempData["ToastType"] = "error";

            model.Departments = _context.DepartmentTbl.ToList();
            return View(model);
        }
    }
}
