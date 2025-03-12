using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Models
{
    public class BatchModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Class is required.")]
        [MaxLength(50, ErrorMessage = "Class names cannot exceed 50 characters.")]
        public string ClassNames { get; set; } // Renamed from "Class" to "ClassName"
    }
}
