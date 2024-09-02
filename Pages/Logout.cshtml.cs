using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace TimeOffManager.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            // Sign out the user with the correct scheme
            await HttpContext.SignOutAsync("Cookies");

            // Redirect to the login page after logout
            return RedirectToPage("/Login");
        }
    }
}

