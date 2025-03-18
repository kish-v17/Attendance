using Attendance.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Models
{
    public class BatchModel
    {
        [Key]
        public int BatchId { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Course ID.")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        [Range(1, 8, ErrorMessage = "Semester must be between 1 and 10.")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "Number of classes is required.")]
        [Range(1, 10, ErrorMessage = "Number of classes must be between 1 and 10.")]
        public int NumberOfClasses { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.Date)]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End Date must be after Start Date.")]
        public DateOnly EndDate { get; set; }
        [ValidateNever]
        public List<CourseModel>? Courses { get; set; } 

    }
}
