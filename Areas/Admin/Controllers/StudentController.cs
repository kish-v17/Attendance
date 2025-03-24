using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Mono.TextTemplating;
using System.Diagnostics.Metrics;
using System.Net;

namespace Attendance.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Create()
        {
            var classList = _context.ClassTbl
            .Include(c => c.Batch)
            .ThenInclude(b => b.Course)
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.ClassId.ToString(),
                Text = c.Batch != null && c.Batch.Course != null
                    ? $"{c.Batch.Year} - {GetShortName(c.Batch.Course.CourseName)} - {c.Batch.Semester} - {c.Class}"
                    : "N/A"
            })
            .Distinct()
            .ToList();

            ViewBag.ClassList = classList;

            return View();

        }
        [HttpPost]
        public IActionResult Create(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                var model=new StudentModel
                {
                    FullName = student.FullName,
                    EnrollmentNumber = GenerateEnrollmentNumber(student.ClassId),
                    FatherName = student.FatherName,
                    MotherName = student.MotherName,
                    Email = student.Email,
                    Gender = student.Gender,
                    DateOfBirth = student.DateOfBirth,
                    AadharCardNumber = student.AadharCardNumber,
                    BloodGroup = student.BloodGroup,
                    MobileNo = student.MobileNo,
                    ParentMobileNo = student.ParentMobileNo,
                    Category = student.Category,
                    Address = student.Address,
                    City = student.City,
                    State = student.State,
                    Country = student.Country,
                    PinCode = student.PinCode,
                    ClassId = student.ClassId

                };
                _context.StudentTbl.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }return View(student);
        }
        
        public IActionResult Edit(int id)
        {
            var student = _context.StudentTbl.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new StudentModel
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                EnrollmentNumber = student.EnrollmentNumber,
                FatherName = student.FatherName,
                MotherName = student.MotherName,
                Email = student.Email,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                AadharCardNumber = student.AadharCardNumber,
                BloodGroup = student.BloodGroup,
                MobileNo = student.MobileNo,
                ParentMobileNo = student.ParentMobileNo,
                Category = student.Category,
                Address = student.Address,
                City = student.City,
                State = student.State,
                Country = student.Country,
                PinCode = student.PinCode,
                ClassId=student.ClassId
            };

            var classList = _context.ClassTbl
         .Include(c => c.Batch)
         .ThenInclude(b => b.Course)
         .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
         {
             Value = c.ClassId.ToString(),
             Text = c.Batch != null && c.Batch.Course != null
                 ? $"{c.Batch.Year}-{GetShortName(c.Batch.Course.CourseName)} - {c.Batch.Semester} - {c.Class}"
                 : "N/A",
             Selected = (c.ClassId == student.ClassId)
         })
         .Distinct()
         .ToList();

            ViewBag.ClassList = classList;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                var student = _context.StudentTbl.Find(model.StudentId);
                if (student == null)
                {
                    return NotFound();
                }
                student.FullName = model.FullName;
                student.FatherName = model.FatherName;
                student.MotherName = model.MotherName;
                student.Email = model.Email;
                student.Gender = model.Gender;
                student.DateOfBirth = model.DateOfBirth;
                student.AadharCardNumber = model.AadharCardNumber;
                student.BloodGroup = model.BloodGroup;
                student.MobileNo = model.MobileNo;
                student.ParentMobileNo = model.ParentMobileNo;
                student.Category = model.Category;
                student.Address = model.Address;
                student.City = model.City;
                student.State = model.State;
                student.Country = model.Country;
                student.PinCode = model.PinCode;
                student.ClassId = model.ClassId;
                _context.StudentTbl.Update(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }return View(model);
        }

        public IActionResult GetAll()
        {
            var students = _context.StudentTbl
                .Include(s => s.Class)
                .ThenInclude(c => c.Batch)
                .ThenInclude(b => b.Course)
                .ToList(); 

            var transformedStudents = students.Select(b => new
            {
                b.StudentId,
                b.FullName,
                b.EnrollmentNumber,
                b.FatherName,
                b.MotherName,
                Gender = b.Gender.ToString(),
                DateOfBirth = b.DateOfBirth.ToString("yyyy-MM-dd"),
                b.AadharCardNumber,
                BloodGroup = b.BloodGroup.ToString(),
                b.Email,
                b.MobileNo,
                b.ParentMobileNo,
                Category = b.Category.ToString(),
                b.Address,
                b.City,
                b.State,
                b.Country,
                b.PinCode,

                ClassName = b.Class != null && b.Class.Batch != null && b.Class.Batch.Course != null
                    ? $"{b.Class.Batch.Year}-{GetShortName(b.Class.Batch.Course.CourseName)} - {b.Class.Batch.Semester} - {b.Class.Class}"
                    : "N/A"
            }).ToList(); 

            return Json(new { data = transformedStudents });
        }

        private static string GetShortName(string courseName)
        {
            if (string.IsNullOrWhiteSpace(courseName)) return "N/A";

            return new string(courseName.Where(char.IsUpper).ToArray());
        }
        private string GenerateEnrollmentNumber(int classId)
        {
            var classEntity = _context.ClassTbl
                .Include(c => c.Batch)
                .ThenInclude(b => b.Course)
                .ThenInclude(c => c.Department)
                .FirstOrDefault(c => c.ClassId == classId);

            if (classEntity == null || classEntity.Batch == null || classEntity.Batch.Course == null)
            {
                throw new Exception("Invalid Class Selection");
            }

            // Extract required details
            string year = classEntity.Batch.Year.ToString().Substring(2, 2); // Get last 2 digits of year
            string departmentCode = GetCapitalLetters(classEntity.Batch.Course.Department.DepartmentName);
            string courseCode = GetCapitalLetters(classEntity.Batch.Course.CourseName);

            // Fetch last assigned enrollment number for this year, department, and course
            string prefix = $"{year}{departmentCode}{courseCode}";

            var lastEnrollment = _context.StudentTbl
                .Where(s => s.EnrollmentNumber.StartsWith(prefix))
                .OrderByDescending(s => s.EnrollmentNumber)
                .Select(s => s.EnrollmentNumber)
                .FirstOrDefault();

            int newNumber = 1; // Start from 0001

            if (lastEnrollment != null)
            {
                string lastDigits = lastEnrollment.Substring(prefix.Length); // Extract last 4 digits
                if (int.TryParse(lastDigits, out int lastNumeric))
                {
                    newNumber = lastNumeric + 1;
                }
            }

            return $"{prefix}{newNumber:D4}"; // Format as 4-digit number
        }
        private string GetCapitalLetters(string input)
        {
            return new string(input.Where(char.IsUpper).ToArray());
        }
    }
}
