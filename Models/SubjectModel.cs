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
        public string SubjectName { get; set; }
        [Required(ErrorMessage = "Short Name is required.")]
        public string SubjectShortName { get; set; }
    }
}
