using Microsoft.AspNetCore.Mvc;

namespace Attendance.Areas.Faculty.Controllers
{
    [Area("Faculty")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
