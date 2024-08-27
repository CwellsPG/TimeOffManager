using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;  // Needed for DateTime

namespace TimeOffManager.Pages  
{
    public class TimeOffRequestModel : PageModel
    {
        [BindProperty]
        public string LeaveType { get; set; } = "";
        [BindProperty]
        public DateTime StartDate { get; set; }
        [BindProperty]
        public DateTime EndDate { get; set; }

        public void OnGet()
        {
            // Method for handling GET requests
            // Additioanl code here in further Sprints
        }

        public IActionResult OnPost()
        {
            // This method handles the form submission when the user submits the time-off request form.
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return to the same page to display validation errors.
                return Page();
            }

            // Additioanl code here in further Sprints - database integration
            
            return RedirectToPage("/Index"); 
        }
    }
}