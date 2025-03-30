using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Attendance.Models
{
    public class AttendanceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceId { get; set; }

        [Required]
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; } 

        [Required]
        public DateTime AttendanceDate { get; set; } = DateTime.Now;

        [Required]
        public AttendanceStatus Status { get; set; }
        [ValidateNever]
        public virtual ScheduleModel? Schedule { get; set; }
        [ValidateNever]
        public virtual StudentModel? Student { get; set; }
    }

    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Suspended = 3,
        Holiday = 4,
    }
}