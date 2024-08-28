namespace TimeOffManager.Models
{
    public class TimeOffRequest
    {
        public int Id { get; set; }
        public string LeaveType { get; set; } = ""; // Default to an empty string
        public DateTime StartDate { get; set; } = DateTime.Now; // Default to current date
        public DateTime EndDate { get; set; } = DateTime.Now; // Default to current date
        public DateTime RequestDate { get; set; } = DateTime.Now; // Default to current date
        public string Status { get; set; } = "Pending"; // Default status
    }
}