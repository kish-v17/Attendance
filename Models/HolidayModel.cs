using System.ComponentModel.DataAnnotations;

namespace Attendance.Models
{
    public class HolidayModel
    {
        [Key]
        public int HolidayId { get; set; }

        [Required]
        public DateTime HolidayDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        public bool IsRecurring { get; set; } 
    }

}
