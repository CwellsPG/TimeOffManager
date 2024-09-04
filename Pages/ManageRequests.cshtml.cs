using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOffManager.Data;
using TimeOffManager.Models;

namespace TimeOffManager.Pages
{
    public class ManageRequestsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageRequestsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<LeaveRequest> LeaveRequests { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch all pending leave requests
            LeaveRequests = await _context.LeaveRequests
                .Include(lr => lr.User)
                .Where(lr => lr.Status == "Pending")
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(int requestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(requestId);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            // Update request status to approved
            leaveRequest.Status = "Approved";

            // Update leave balance for the user
            var leaveBalance = await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.UserId == leaveRequest.UserId && lb.LeaveType == leaveRequest.LeaveType);

            if (leaveBalance != null)
            {
                int daysRequested = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1; // Include start day
                leaveBalance.RemainingDays -= daysRequested;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(int requestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(requestId);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            // Update request status to rejected
            leaveRequest.Status = "Denied";

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
