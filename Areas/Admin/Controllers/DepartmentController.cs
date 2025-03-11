using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Areas.Admin.Controllers
{
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
                d.DepartmentName
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
                return RedirectToAction("Index");
            }
            return View(department);
        }

        public IActionResult Edit(int id)
        {
            var department = _context.DepartmentTbl.Find(id);
            if (department == null)
            {
                return NotFound();
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
                return RedirectToAction("Index");
            }
            return View(department);
        }

        
    }
}
