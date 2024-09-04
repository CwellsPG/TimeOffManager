using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeOffManager.Data;
using TimeOffManager.Models;

namespace TimeOffManager.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            // Method for handling GET requests
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Query the database to authenticate the user
            var user = await _context.Users
                .Include(u => u.Role)  
                .FirstOrDefaultAsync(u => u.Email == Email && u.Password == Password);

            if (user != null) // If a user is found
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.RoleName),  
                    new Claim("UserId", user.UserId.ToString()) // Storing the UserId in a claim
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return RedirectToPage("/Index"); // Redirect to the home page or another dashboard page after successful login
            }

            ErrorMessage = "Invalid login attempt.";
            return Page();
        }
    }
}





