using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly AppDBContext _context;

        public DepartmentController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var departments = _context.DepartmentTbl.Select(d => new
            {
                d.DepartmentId,
                d.DepartmentName,
                d.DepartmentShortName
            }).ToList();

            return Json(new { data = departments });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentModel department)
        {
            if (ModelState.IsValid)
            {
                _context.DepartmentTbl.Add(department);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Department added successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to added department. Please check the input.";
            TempData["ToastType"] = "error";

            return View(department);
        }

        public IActionResult Edit(int id)
        {
            var department = _context.DepartmentTbl.Find(id);
            if (department == null)
            {
                TempData["ToastMessage"] = "Department not found!";
                TempData["ToastType"] = "error";

                return RedirectToAction("Index");
            }

            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(DepartmentModel department)
        {
            if (ModelState.IsValid)
            {
                _context.DepartmentTbl.Update(department);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Department updated successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to update department. Please check the input.";
            TempData["ToastType"] = "error";

            return View(department);
        }
    }
}
