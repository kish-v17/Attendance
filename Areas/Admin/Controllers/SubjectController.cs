using Microsoft.AspNetCore.Mvc;

namespace Attendance.Areas.Admin.Controllers
{
    public class SubjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
