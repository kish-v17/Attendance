using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDBContext _db; 
        public AuthController(AppDBContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid)
            {
                return View(lvm);
            }

            var user = _db.UserTbl
                .FirstOrDefault(u => u.Email == lvm.Email && u.Password == lvm.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Fullname", user.Fullname);
                HttpContext.Session.SetString("Role", user.Role.ToString());

                // Redirect based on user role
                return user.Role switch
                {
                    UserRole.Admin => RedirectToAction("Index", "Home", new { area = "Admin" }),
                    UserRole.Faculty => RedirectToAction("Index", "Home", new { area = "Faculty" }),
                };
                //return (user.Role == UserRole.Admin)
                //? RedirectToAction("Index", "Home", new { area = "Admin" })
                //: RedirectToAction("Index", "Home", new { area = "Faculty" });
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(lvm);
        }
        
    }
}
