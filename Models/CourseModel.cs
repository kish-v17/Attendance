using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    }
}

