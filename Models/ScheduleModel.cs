using Attendance.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace Attendance.Models
{

    public class ScheduleModel
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Subject.")]
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        [ValidateNever]
        public SubjectModel Subject { get; set; }

        [Required(ErrorMessage = "Faculty is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Faculty.")]
        public int FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        [ValidateNever]
        public UserModel User { get; set; }


        [Required(ErrorMessage = "Class is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Class.")]
        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        [ValidateNever]
        public ClassModel? Class { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Day is required.")]
        [EnumDataType(typeof(DaysOfWeek), ErrorMessage = "Invalid Day.")]
        public DaysOfWeek Day { get; set; }
    }

    public enum DaysOfWeek
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
    }
}