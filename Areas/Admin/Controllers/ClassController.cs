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
        public IActionResult Index(string context)
        {
            ViewData["Context"] = context;
            return View();
        }
        public IActionResult GetAll()
        {
            var classes = _context.ClassTbl
                .Include(b => b.Batch)
                    .ThenInclude(c => c.Course)
                        .ThenInclude(d => d.Department)
                .Select(c => new
                {
                    c.ClassId,
                    CourseName = c.Batch.Course.CourseName + " (" + c.Batch.Course.CourseShortName + ")",
                    Department = c.Batch.Course.Department.DepartmentShortName,
                    c.Batch.Semester,
                    c.ClassName,

                }).ToList();

            return Json(new { data = classes });
        }
    }
}
