using Attendance.Data;
using Attendance.Models;
using Attendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HolidayController : Controller
    {
        private readonly AppDBContext _context;
        public HolidayController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var users = _context.HolidayTbl
                .Select(c => new
                {
                    c.HolidayId,
                    c.Title,
                    HolidayDate=c.HolidayDate.ToShortDateString(),
                    c.Description,
                    c.IsRecurring,
                }).ToList();

            return Json(new { data = users });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(HolidayModel holiday)
        {
            if (ModelState.IsValid)
            {
                _context.HolidayTbl.Add(holiday);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Department added successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to added department. Please check the input.";
            TempData["ToastType"] = "error";

            return View(holiday);
        }
        public IActionResult Edit(int id)
        {
            var holiday = _context.HolidayTbl.Find(id);
            if (holiday == null)
            {
                TempData["ToastMessage"] = "Holiday not found!";
                TempData["ToastType"] = "error";

                return RedirectToAction("Index");
            }

            return View(holiday);
        }

        [HttpPost]
        public IActionResult Edit(HolidayModel holiday)
        {
            if (ModelState.IsValid)
            {
                _context.HolidayTbl.Update(holiday);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Holiday updated successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to update holiday. Please check the input.";
            TempData["ToastType"] = "error";

            return View(holiday);
        }
    }
}
