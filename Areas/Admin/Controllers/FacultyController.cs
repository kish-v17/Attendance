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
    public class FacultyController : Controller
    {
        private readonly AppDBContext _context;
        private readonly EmailService _emailService;

        public FacultyController(AppDBContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var users = _context.UserTbl.Where(c => c.Role == UserRole.Faculty)
                .Select(c => new
                {
                    c.UserId,
                    c.Fullname,
                    c.MobileNo,
                    c.Email
                }).ToList();

            return Json(new { data = users });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserModel user)
        {
            user.Password = generatePassword();

            if (ModelState.IsValid)
            {
                string msg = $"Dear {user.Fullname},\n\nWelcome to RK University!\n\n" +
                      $"Your faculty account has been successfully created. Please find your login credentials below:\n\n" +
                      $"Username: {user.Email}\nPassword: *{user.Password}*\n\n" +
                      $"We recommend changing your password after your first login.\n\nRegards,\nAdmin Team";

                _emailService.SendEmail(user.Email, "Welcome to RK University - Login Details", msg);

                _context.UserTbl.Add(user);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Faculty created and email sent successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to create faculty. Please check the form.";
            TempData["ToastType"] = "error";

            return View(user);
        }

        public IActionResult Edit(int id)
        {
            var faculty = _context.UserTbl.Find(id);
            if (faculty == null)
            {
                TempData["ToastMessage"] = "Faculty not found.";
                TempData["ToastType"] = "error";
                return NotFound();
            }

            var viewModel = new UserModel
            {
                UserId = faculty.UserId,
                Fullname = faculty.Fullname,
                Email = faculty.Email,
                MobileNo = faculty.MobileNo,
                Role = faculty.Role,
                Password = faculty.Password
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserTbl.Find(model.UserId);
                if (user == null)
                {
                    TempData["ToastMessage"] = "Faculty not found.";
                    TempData["ToastType"] = "error";
                    return NotFound();
                }

                user.Fullname = model.Fullname;
                user.Email = model.Email;
                user.MobileNo = model.MobileNo;

                _context.UserTbl.Update(user);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Faculty updated successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to update faculty. Please check the form.";
            TempData["ToastType"] = "error";

            return View(model);
        }

        public static string generatePassword()
        {
            const string specialChars = "!@#$%^&*()-_=+<>?";
            Random random = new Random();
            char specialChar = specialChars[random.Next(specialChars.Length)];
            int randomNumber = random.Next(100, 1000);
            string password = $"RKU{specialChar}{randomNumber}";
            return password;
        }
    }
}
