using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Application.Interfaces.Repositories.IMedSchedRepository;

namespace WebUi.Pages.Appointment;

public class IndexModel(IMedSchedRepository repository) : PageModel
{
    public GetAllAppointmentDto Appointments { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    [Range(minimum: 0, maximum: int.MaxValue)]
    public int Skip { get; set; } = 0;

    [BindProperty(SupportsGet = true)]
    public int Top { get; set; } = 10;
    public bool HasNext => (Skip + 1) * Top < Appointments.Count;
    public async Task<IActionResult> OnGet(int skip, CancellationToken cancellationToken, int top = 10)
    {
        Skip = skip == -1 ? 0 : skip;
        Top = top;
        var userId = User?.FindFirst("sub")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var physicianId))
        {
            return RedirectToPage("/");
        }
        var result = await repository.GetAllAppointmentsAsync(top, physicianId!, null, null, Skip, cancellationToken);
        Appointments = result.ValueOrDefault ?? new GetAllAppointmentDto([], 0);
        return Page();
    }
}
