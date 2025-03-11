using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Models.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; } // Required for editing
        [Required, MaxLength(100)]
        public string CourseName { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        [ValidateNever]
        public List<DepartmentModel> Departments { get; set; }

    }

}
