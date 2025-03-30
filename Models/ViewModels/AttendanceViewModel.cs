namespace Attendance.Models.ViewModels
{
    public class AttendanceViewModel
    {
        public int AttendanceId { get; set; }
        public string Enrolment { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string ClassName { get; set; }
        public string Duration { get; set; }
        public string AttendanceDate { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
