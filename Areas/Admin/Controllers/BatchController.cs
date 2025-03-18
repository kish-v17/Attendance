﻿using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Attendance.Areas.Admin.Controllers
{
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
                    CourseName = b.Course.CourseName
                }).ToList();

            return Json(new { data = batches });
        }

        public IActionResult Create()
        {
            var viewModel = new BatchModel
            {
                Courses = _context.CourseTbl.ToList() // Fetch courses from DB
            };
            return View(viewModel);
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
                    Year = model.Year, // Added Year
                    CourseId = model.CourseId
                };

                _context.BatchTbl.Add(batch);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // If validation fails, reload the course list
            model.Courses = _context.CourseTbl.ToList();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var batch = _context.BatchTbl.Find(id);
            if (batch == null)
            {
                return NotFound();
            }

            var viewModel = new BatchModel
            {
                BatchId = batch.BatchId,
                Semester = batch.Semester,
                StartDate = batch.StartDate,
                EndDate = batch.EndDate,
                Year = batch.Year, 
                CourseId = batch.CourseId,
                Courses = _context.CourseTbl.ToList()

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
                    return NotFound();
                }

                batch.Semester = model.Semester;
                batch.StartDate = model.StartDate;
                batch.EndDate = model.EndDate; // Added ClassName
                batch.Year = model.Year; // Added Year
                batch.CourseId = model.CourseId;

                _context.BatchTbl.Update(batch);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // If validation fails, reload the course list
            model.Courses = _context.CourseTbl.ToList();
            return View(model);
        }
    }
}