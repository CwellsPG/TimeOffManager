namespace TimeOffManager.Models
{
    public class TimeOffRequest
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}