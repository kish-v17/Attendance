using Attendance.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ClassController : Controller
    {
        private readonly AppDBContext _context;

        public ClassController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult GetAll()
        //{
            

        //    return Json(new { });
        //}
    }
}
