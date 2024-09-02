using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeOffManager.Data;
using TimeOffManager.Models;

namespace TimeOffManager.Pages
{
    public class TimeOffRequestModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TimeOffRequestModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string LeaveType { get; set; } = "";

        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now;

        public List<LeaveBalance> LeaveBalances { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; }
        public string ConflictMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the user ID from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return RedirectToPage("/Login");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Load leave balances from the database
            LeaveBalances = await _context.LeaveBalances
                .Where(lb => lb.UserId == userId)
                .ToListAsync();

            // Load existing time-off requests for conflict checking
            LeaveRequests = await _context.LeaveRequests
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            System.Diagnostics.Debug.WriteLine("OnPost method called.");
            System.Diagnostics.Debug.WriteLine($"Submitted LeaveType: {LeaveType}");
            System.Diagnostics.Debug.WriteLine($"Submitted StartDate: {StartDate}");
            System.Diagnostics.Debug.WriteLine($"Submitted EndDate: {EndDate}");

            // Retrieve the user ID from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return RedirectToPage("/Login");
            }

            int userId = int.Parse(userIdClaim.Value);

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is not valid.");
                return Page();
            }

            System.Diagnostics.Debug.WriteLine("ModelState is valid, checking for conflicts.");

            if (await CheckForConflictsAsync(userId))
            {
                System.Diagnostics.Debug.WriteLine("Conflict detected.");
                ConflictMessage = "There is a conflict with your time-off request. Please contact your manager for further assistance.";
                return Page();  // Stay on the same page to show the conflict message
            }

            // Save the new time-off request to the database
            var newRequest = new LeaveRequest
            {
                UserId = userId,
                LeaveType = LeaveType,
                StartDate = StartDate,
                EndDate = EndDate,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            _context.LeaveRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            System.Diagnostics.Debug.WriteLine("No conflict detected. Request saved and redirecting to Index.");

            return RedirectToPage("/Index");
        }

        private async Task<bool> CheckForConflictsAsync(int userId)
        {
            System.Diagnostics.Debug.WriteLine("CheckForConflicts method called.");

            // Fetch existing requests that overlap with the requested dates
            var conflictingRequests = await _context.LeaveRequests
                .Where(r => r.LeaveType == LeaveType &&
                            r.UserId == userId &&
                            r.StartDate <= EndDate &&
                            r.EndDate >= StartDate)
                .ToListAsync();

            if (conflictingRequests.Any())
            {
                System.Diagnostics.Debug.WriteLine("Conflict detected!");
                return true; // Conflict found
            }

            System.Diagnostics.Debug.WriteLine("No conflict detected.");
            return false; // No conflicts
        }

        public async Task<IActionResult> ApproveLeaveRequest(int requestId)
        {
            // Find the leave request by ID
            var leaveRequest = await _context.LeaveRequests.FindAsync(requestId);

            if (leaveRequest == null || leaveRequest.Status == "Approved")
            {
                // Handle case where request is not found or already approved
                return NotFound();
            }

            // Find the user's leave balance for the specific LeaveType
            var leaveBalance = await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.UserId == leaveRequest.UserId && lb.LeaveType == leaveRequest.LeaveType);

            if (leaveBalance == null || leaveBalance.RemainingDays < (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1)
            {
                // Handle case where leave balance is insufficient
                return BadRequest("Insufficient leave balance.");
            }

            // Deduct the number of days requested from the leave balance
            int daysRequested = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1; // Include start day
            leaveBalance.RemainingDays -= daysRequested;

            // Update the request status to "Approved"
            leaveRequest.Status = "Approved";

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index"); // Redirect to an appropriate page after approval
        }
    }
}






