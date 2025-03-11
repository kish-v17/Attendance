using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Models
{
    public class SubjectModel
    {
        [Key]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Subject Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Subject Name must be between {2} and {1} characters.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Subject Name can only contain letters and spaces.")]
        public string SubjectName { get; set; }
    }
}
