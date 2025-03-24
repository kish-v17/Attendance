using Attendance.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
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
