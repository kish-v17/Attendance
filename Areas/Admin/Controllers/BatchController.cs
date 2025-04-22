using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Attendance.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class BatchController : Controller
    {
        private readonly AppDBContext _context;

        public BatchController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var batches = _context.BatchTbl.Include(b => b.Course)
                .Select(b => new
                {
                    b.BatchId,
                    b.Semester,
                    b.StartDate,
                    b.EndDate,
                    b.Year,
                    b.NumberOfClasses,
                    CourseName = b.Course.CourseName
                }).ToList();

            return Json(new { data = batches });
        }

        public IActionResult Create()
        {
            var batch = new BatchModel
            {
                Courses = _context.CourseTbl
                .Include(c => c.Department)
                .AsEnumerable()
                .Select(c => new CourseModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName + " - " + c.Department.DepartmentShortName
                })
                .ToList()
            };
            return View(batch);
        }

        [HttpPost]
        public IActionResult Create(BatchModel model)
        {
            if (ModelState.IsValid)
            {
                var batch = new BatchModel
                {
                    Semester = model.Semester,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Year = model.Year,
                    NumberOfClasses = model.NumberOfClasses,
                    CourseId = model.CourseId
                };

                _context.BatchTbl.Add(batch);
                _context.SaveChanges();

                List<ClassModel> classList = new List<ClassModel>();
                for (int i = 0; i < model.NumberOfClasses; i++)
                {
                    classList.Add(new ClassModel
                    {
                        BatchId = batch.BatchId, // Note: use batch.BatchId (not model.BatchId)
                        ClassName = ((char)('A' + i)).ToString()
                    });
                }

                _context.ClassTbl.AddRange(classList);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Batch created successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to create batch. Please check the entered details.";
            TempData["ToastType"] = "error";

            model.Courses = _context.CourseTbl.ToList();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var batch = _context.BatchTbl.Find(id);
            if (batch == null)
            {
                TempData["ToastMessage"] = "Batch not found!";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }

            var viewModel = new BatchModel
            {
                BatchId = batch.BatchId,
                Semester = batch.Semester,
                StartDate = batch.StartDate,
                EndDate = batch.EndDate,
                Year = batch.Year,
                NumberOfClasses = batch.NumberOfClasses,
                CourseId = batch.CourseId,
                Courses = _context.CourseTbl
                    .Include(c => c.Department)
                    .AsEnumerable()
                    .Select(c => new CourseModel
                    {
                        CourseId = c.CourseId,
                        CourseName = c.CourseName + " - " + c.Department.DepartmentShortName
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(BatchModel model)
        {
            if (ModelState.IsValid)
            {
                var batch = _context.BatchTbl.Find(model.BatchId);
                if (batch == null)
                {
                    TempData["ToastMessage"] = "Batch not found!";
                    TempData["ToastType"] = "error";
                    return RedirectToAction("Index");
                }

                batch.Semester = model.Semester;
                batch.StartDate = model.StartDate;
                batch.EndDate = model.EndDate;
                batch.Year = model.Year;
                batch.CourseId = model.CourseId;
                batch.NumberOfClasses = model.NumberOfClasses;

                _context.BatchTbl.Update(batch);
                _context.SaveChanges();

                TempData["ToastMessage"] = "Batch updated successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Failed to update batch. Please check the entered details.";
            TempData["ToastType"] = "error";

            model.Courses = _context.CourseTbl.ToList();
            return View(model);
        }
    }
}
