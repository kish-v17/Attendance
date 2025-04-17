using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Mono.TextTemplating;
using System.Diagnostics.Metrics;
using System.Net;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly AppDBContext _context;

        public StudentController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            var classInfo = _context.ClassTbl
                .Include(c => c.Batch)
                    .ThenInclude(b => b.Course)
                .FirstOrDefault(c => c.ClassId == id);

            if (classInfo != null)
            {
                ViewData["ClassId"] = id;
                ViewData["ClassName"] = $"{classInfo.Batch.Course.CourseShortName} -  {classInfo.Batch.Semester} - {classInfo.ClassName}";
            }

            return View();
        }
        public IActionResult Create(int? classId)
        {
            ViewBag.ClassList = _context.ClassTbl
                .Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.Batch.Course.CourseShortName + " - Sem " + c.Batch.Semester + " - " + c.ClassName,
                    Selected = (classId != null && c.ClassId == classId)
                })
                .ToList();

            var model = new StudentModel();
            if (classId != null)
            {
                model.ClassId = classId.Value;
            }

            return View(model);
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
                TempData["ToastMessage"] = "Student added successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Index", new { id = student.ClassId });
            }
            return View(student);
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
             Text = c.Batch.Course.CourseShortName + " - Sem " + c.Batch.Semester + " - " + c.ClassName,
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

                TempData["ToastMessage"] = "Student updated successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Index", new { id = student.ClassId });
            }
            TempData["ToastMessage"] = "Student not found.";
            TempData["ToastType"] = "error";
            return View(model);
        }

        public IActionResult GetAll(int classId)
        {
            var students = _context.StudentTbl
                .Where(s => s.ClassId == classId)
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
                    ? $"{b.Class.Batch.Year}-{b.Class.Batch.Course.CourseShortName} - {b.Class.Batch.Semester} - {b.Class.ClassName}"
                    : "N/A"

            }).ToList(); 
            return Json(new { data = transformedStudents });
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

            string year = classEntity.Batch.Year.ToString().Substring(2, 2); // Get last 2 digits of year
            string departmentCode = GetCapitalLetters(classEntity.Batch.Course.Department.DepartmentName);
            string courseCode = GetCapitalLetters(classEntity.Batch.Course.CourseName);

            string prefix = $"{year}{departmentCode}{courseCode}";

            var lastEnrollment = _context.StudentTbl
                .Where(s => s.EnrollmentNumber.StartsWith(prefix))
                .OrderByDescending(s => s.EnrollmentNumber)
                .Select(s => s.EnrollmentNumber)
                .FirstOrDefault();

            int newNumber = 1; 

            if (lastEnrollment != null)
            {
                string lastDigits = lastEnrollment.Substring(prefix.Length); 
                if (int.TryParse(lastDigits, out int lastNumeric))
                {
                    newNumber = lastNumeric + 1;
                }
            }

            return $"{prefix}{newNumber:D4}"; 
        }
        private string GetCapitalLetters(string input)
        {
            return new string(input.Where(char.IsUpper).ToArray());
        }
    }
}
