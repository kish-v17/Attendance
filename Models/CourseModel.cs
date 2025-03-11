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

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public DepartmentModel Department { get; set; }
    }
}

