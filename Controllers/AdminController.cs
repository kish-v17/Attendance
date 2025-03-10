using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _db;

        public AdminController(AppDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            List<UserModel> user = _db.UserTbl.ToList();
            return View(user);
        }
        public IActionResult AddUser()
        {
            return View();
        }

    }
}
