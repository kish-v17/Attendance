using System.ComponentModel.DataAnnotations;

namespace Attendance.Models
{
    public class DepartmentModel
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required, MaxLength(100)]
        public string DepartmentName { get; set; }
    }

}
