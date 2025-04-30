using Attendance.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Models
{
    public class LectureStatusModel
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime FillDate { get; set; }

        public LectureStatusEnum Status { get; set; }

        public String Remarks { get; set; }
        [ValidateNever]
        [ForeignKey("ScheduleId")]
        public ScheduleModel Schedule { get; set; }
    }
    public enum LectureStatusEnum
    {
        Pending = 1,
        Filled = 2,
        Suspended = 3,
        Holiday = 4
    }
}
