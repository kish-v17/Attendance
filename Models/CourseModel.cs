using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Attendance.Models
{
    public class CourseModel
    {
        [Key]
        public int CourseId { get; set; }

        [Required, MaxLength(100)]
        public string CourseName { get; set; } 
        [Required, MaxLength(100)]
        public string CourseShortName { get; set; } 
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        [ValidateNever]
        public DepartmentModel Department { get; set; }
        [ValidateNever]
        public List<DepartmentModel> Departments { get; set; }
    }
}

