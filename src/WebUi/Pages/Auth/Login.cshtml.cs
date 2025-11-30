using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebUi.Extensions;

namespace WebUi.Pages.Auth;

public class LoginModel(IAuthRepository authRepository) : PageModel
{
    private readonly IAuthRepository _authRepository = authRepository;
    [BindProperty]
    [Required, EmailAddress]
    public required string Username { get; set; }
    [BindProperty]
    [Required, DataType(DataType.Password)]
    public required string Password { get; set; }
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");
        return Page();
    }
    public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var result = await _authRepository.LoginAsync(new IAuthRepository.LoginRequest(Username!, Password!), cancellationToken);
        if (result.IsFailed)
        {
            ModelState.AddModelError(string.Empty, result.Errors[0]!.Message);
            return Page();
        }
        var claims = JwtClaimsParser.ToClaimsPrincipal(result.Value!.AccessToken);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claims,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(result.Value!.ExpiresIn)
            });
        return RedirectToPage("/Index");
    }
}
