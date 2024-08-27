using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks; // Ensure you include this for Task support

namespace TimeOffManager.Pages  
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            // Method for handling GET requests
            // Additioanl code here in further Sprints
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Username == "admin" && Password == "password") // Simplified for example purposes
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, Username) };
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                return RedirectToPage("/Index"); // Redirect to the index page or another dashboard page after successful login
            }

            ErrorMessage = "Invalid login attempt.";
            return Page();
        }
    }
}