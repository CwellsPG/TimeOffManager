using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;  // Needed for DateTime

namespace TimeOffManager.Pages  // Adjust the namespace to match your project structure
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
            // You can include initialization code here if needed
        }

        public IActionResult OnPost()
        {
            // This method handles the form submission when the user submits the time-off request form.
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return to the same page to display validation errors.
                return Page();
            }

            // Since there's no database integration yet, you could log these values, show a confirmation message, or simply redirect
            // For demonstration, let's assume you redirect to a confirmation or index page after successful submission
            return RedirectToPage("/Index"); // Adjust the redirection to wherever you'd like the user to go post-submission
        }
    }
}