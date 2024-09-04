using System.ComponentModel.DataAnnotations;

namespace TimeOffManager.Models
{
    public class LeaveRequest
    {
        [Key] // Mark RequestId as the primary key
        public int RequestId { get; set; } // Primary key

        public int UserId { get; set; } // User who made the request
        public User User { get; set; } // Navigation property for the user who made the request

        public string LeaveType { get; set; } = ""; // Type of leave (Vacation, Sick, etc.)
        public DateTime StartDate { get; set; } = DateTime.Now; // Start date of the leave
        public DateTime EndDate { get; set; } = DateTime.Now; // End date of the leave
        public string Status { get; set; } = "Pending"; // Status of the request (Pending, Approved, Denied)
        public DateTime RequestDate { get; set; } = DateTime.Now; // Date when the leave was requested

        public int? ApprovedBy { get; set; } // Nullable for when not yet approved
        public User ApprovedByUser { get; set; } 
    }
}






