namespace TimeOffManager.Models
{
    public class LeaveBalance
    {
        public string LeaveType { get; set; } = ""; // Default to an empty string
        public int RemainingDays { get; set; } = 1; // Default to 1 for demo
    }
}