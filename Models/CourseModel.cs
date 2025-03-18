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
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public DepartmentModel Department { get; set; }
        [ValidateNever]
        public List<DepartmentModel> Departments { get; set; }
    }
}

