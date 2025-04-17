using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class SubjectController : Controller
    {
        private readonly AppDBContext _context;

        public SubjectController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var subjects = _context.SubjectTbl.Select(s => new
            {
                s.SubjectId,
                s.SubjectName,
                s.SubjectShortName
            }).ToList();

            return Json(new { data = subjects });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SubjectModel subject)
        {
            if (ModelState.IsValid)
            {
                _context.SubjectTbl.Add(subject);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Subject created successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to create subject. Please check the input.";
            TempData["ToastType"] = "error";

            return View(subject);
        }

        public IActionResult Edit(int id)
        {
            var subject = _context.SubjectTbl.Find(id);
            if (subject == null)
            {
                TempData["ToastMessage"] = "Subject not found!";
                TempData["ToastType"] = "error";

                return RedirectToAction("Index");
            }

            return View(subject);
        }

        [HttpPost]
        public IActionResult Edit(SubjectModel subject)
        {
            if (ModelState.IsValid)
            {
                _context.SubjectTbl.Update(subject);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Subject updated successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to update subject. Please check the input.";
            TempData["ToastType"] = "error";

            return View(subject);
        }
    }
}
