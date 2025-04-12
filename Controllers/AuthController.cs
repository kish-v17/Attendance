using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Attendance.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDBContext _db; 
        public AuthController(AppDBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Login()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = User.FindFirstValue(ClaimTypes.Role)
            });
            }

            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.UserTbl.SingleOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.Fullname);
                    HttpContext.Session.SetString("UserRole", user.Role.ToString());
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()) 
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties).Wait();
                    Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                    Response.Headers["Pragma"] = "no-cache";
                    Response.Headers["Expires"] = "0";
                    switch (user.Role.ToString())
                    {
                        case "Admin":
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        case "Faculty":
                            return RedirectToAction("Index", "Home", new { area = "Faculty" });
                        default:
                            return RedirectToAction("Login", "Auth");
                    }
                }
                ModelState.AddModelError("", "Invalid email or password");
            }
            return View(model);
        }
        public async Task<IActionResult> AccessDenied()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Login", "Auth");
        }

    }
}
