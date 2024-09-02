using System.Collections.Generic; // Required for ICollection
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOffManager.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public Role Role { get; set; } // Navigation property to access Role details

        // Navigation property to access the associated LeaveBalances
        public ICollection<LeaveBalance> LeaveBalances { get; set; }
    }
}



