using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
