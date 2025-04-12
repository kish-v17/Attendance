namespace Attendance.Models.ViewModels
{
    public class DashboardViewModel
    {
        public DateOnly Date { get; set; }
        public string ClassName { get; set; }
        public string Subject { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<PendingStudent> PendingStudents { get; set; }
    }

    public class PendingStudent
    {
        public string StudentName { get; set; }
        public string EnrollmentNo { get; set; }
    }

}
