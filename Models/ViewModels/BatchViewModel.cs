using System.ComponentModel.DataAnnotations;
using Attendance.Models;

namespace Attendance.Models.ViewModels
{
    public class BatchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        public int CourseId { get; set; }

        public List<CourseModel>? Courses { get; set; } // To populate dropdown in the view

        [Required(ErrorMessage = "Semester is required.")]
        public int Semester { get; set; } // Changed from int to string


        [Required(ErrorMessage = "Year is required.")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Start date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Class name is required.")]
        [MaxLength(50, ErrorMessage = "Class name cannot exceed 50 characters.")]
        public string ClassNames { get; set; } // Added ClassName field
    }
}
