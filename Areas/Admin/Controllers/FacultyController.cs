using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FacultyController : Controller
    {
        private readonly AppDBContext _context;

        public FacultyController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var users = _context.UserTbl.Where(c=>c.Role==UserRole.Faculty)
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
                _context.UserTbl.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult Edit(int id)
        {
            var faculty = _context.UserTbl.Find(id);
            if (faculty == null)
            {
                return NotFound();
            }
            var viewModel = new UserModel
            {   
                UserId=faculty.UserId,
                Fullname=faculty.Fullname,
                Email=faculty.Email,
                MobileNo=faculty.MobileNo,
                Role=faculty.Role,
                Password=faculty.Password
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserTbl.Find(model.UserId);
                if (user==null)
                {
                    return NotFound();
                }
                user.Fullname= model.Fullname;
                user.Email= model.Email;
                user.MobileNo= model.MobileNo;
                _context.UserTbl.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
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
