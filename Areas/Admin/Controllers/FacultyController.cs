using Microsoft.AspNetCore.Mvc;

namespace Attendance.Areas.Admin.Controllers
{
    public class FacultyController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
