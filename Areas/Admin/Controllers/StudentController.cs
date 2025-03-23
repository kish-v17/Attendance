using Attendance.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Areas.Admin.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDBContext _context;

        public StudentController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var students = _context.StudentTbl.Select(b => new
                {
                    b.StudentId,
                    b.FullName,
                    b.EnrollmentNumber,
                    b.FatherName,
                    b.MotherName,
                    b.Gender,
                    b.DateOfBirth,
                    b.AadharCardNumber,
                    b.BloodGroup,
                    b.Email,
                    b.MobileNo,
                    b.ParentMobileNo,
                    b.Category,
                    b.ClassId,
                    b.Address,
                    b.City,
                    b.State,
                    b.Country,
                    b.PinCode
                }).ToList();

            return Json(new { data = students });
        }
    }
}
