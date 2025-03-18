using System.ComponentModel.DataAnnotations;

namespace Attendance.Models
{
    public class DepartmentModel
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(100, ErrorMessage = "Department Name must be between {2} and {1} characters.", MinimumLength = 3)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Department Name can only contain letters and spaces.")]
        public string DepartmentName { get; set; }

    }
}
