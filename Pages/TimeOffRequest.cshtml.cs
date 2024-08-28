using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using TimeOffManager.Models;

namespace TimeOffManager.Pages
{
    public class TimeOffRequestModel : PageModel
    {
        [BindProperty]
        public string LeaveType { get; set; } = "";

        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now;

        public List<LeaveBalance> LeaveBalances { get; set; }  // For displaying leave balances
        public List<TimeOffRequest> TimeOffRequests { get; set; } // For checking conflicts

        public string ConflictMessage { get; set; }  // For displaying conflict messages

        // To initialize the list
        public TimeOffRequestModel()
        {
            // To avoid null reference
            TimeOffRequests = new List<TimeOffRequest>();
        }

        public void OnGet()
        {
            LeaveBalances = new List<LeaveBalance>
    {
        new LeaveBalance { LeaveType = "Vacation", RemainingDays = 10 },
        new LeaveBalance { LeaveType = "Sick Leave", RemainingDays = 5 },
        new LeaveBalance { LeaveType = "Personal Day", RemainingDays = 2 }
    };

            // Manually hardcode specific dates for testing
            var startDate = new DateTime(2024, 8, 29);
            var endDate = new DateTime(2024, 9, 2);

            TimeOffRequests.Add(new TimeOffRequest
            {
                LeaveType = "Vacation",
                StartDate = startDate,
                EndDate = endDate
            });

            System.Diagnostics.Debug.WriteLine($"Hardcoded Request StartDate: {startDate}");
            System.Diagnostics.Debug.WriteLine($"Hardcoded Request EndDate: {endDate}");
            System.Diagnostics.Debug.WriteLine("OnGet called. Hardcoded TimeOffRequests initialized.");
        }

        public IActionResult OnPost()
        {
            System.Diagnostics.Debug.WriteLine("OnPost method called.");
            System.Diagnostics.Debug.WriteLine($"Submitted LeaveType: {LeaveType}");
            System.Diagnostics.Debug.WriteLine($"Submitted StartDate: {StartDate}");
            System.Diagnostics.Debug.WriteLine($"Submitted EndDate: {EndDate}");

            // Reinitialize LeaveBalances
            LeaveBalances = new List<LeaveBalance>
    {
        new LeaveBalance { LeaveType = "Vacation", RemainingDays = 10 },
        new LeaveBalance { LeaveType = "Sick Leave", RemainingDays = 5 },
        new LeaveBalance { LeaveType = "Personal Day", RemainingDays = 2 }
    };

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is not valid.");
                return Page();
            }

            System.Diagnostics.Debug.WriteLine("ModelState is valid, checking for conflicts.");

            if (CheckForConflicts())
            {
                System.Diagnostics.Debug.WriteLine("Conflict detected.");
                ConflictMessage = "There is a conflict with your time-off request. Please contact your manager for further assistance.";
                return Page();  // Stay on the same page to show the conflict message
            }

            System.Diagnostics.Debug.WriteLine("No conflict detected. Redirecting to Index.");

            return RedirectToPage("/Index");
        }



        private bool CheckForConflicts()
        {
            System.Diagnostics.Debug.WriteLine("CheckForConflicts method called.");

            // Manually create a single hardcoded request for testing
            var hardcodedRequest = new TimeOffRequest
            {
                LeaveType = "Vacation",
                StartDate = new DateTime(2024, 8, 29),
                EndDate = new DateTime(2024, 9, 2)
            };

            // Directly compare against this one request
            System.Diagnostics.Debug.WriteLine($"Comparing with hardcoded request: {hardcodedRequest.LeaveType} from {hardcodedRequest.StartDate} to {hardcodedRequest.EndDate}");

            bool isSameLeaveType = hardcodedRequest.LeaveType.ToLower() == LeaveType.ToLower();
            bool isOverlapping = StartDate.Date <= hardcodedRequest.EndDate.Date && EndDate.Date >= hardcodedRequest.StartDate.Date;

            if (isSameLeaveType && isOverlapping)
            {
                System.Diagnostics.Debug.WriteLine("Conflict detected!");
                return true; // Conflict found
            }

            System.Diagnostics.Debug.WriteLine("No conflict detected.");
            return false; // No conflicts
        }


    }
}


