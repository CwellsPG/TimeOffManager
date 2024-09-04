namespace TimeOffManager.Models
{
    public class LeaveBalance
    {
        public int LeaveBalanceId { get; set; } // Primary key

        public int UserId { get; set; } // Foreign key to the User table
        public User User { get; set; } // Navigation property to link with the User

        public string LeaveType { get; set; } = ""; // Leave type (Vacation, Sick Leave, etc.)
        public int RemainingDays { get; set; } = 1; // Remaining days for this leave type
    }
}
