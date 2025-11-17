using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUi.Pages.Auth;

public class SignoutModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToPage("/Index");
    }
}
