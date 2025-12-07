using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Application.Interfaces.Repositories.IMedSchedRepository;

namespace WebUi.Pages.Schedule;

public class IndexModel(IMedSchedRepository repository) : PageModel
{
    public IEnumerable<ScheduleDto> Schedules { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    [Range(minimum: 0, maximum: int.MaxValue)]
    public int Skip { get; set; } = 0;

    [BindProperty(SupportsGet = true)]
    public int Top { get; set; } = 20;
    public async Task<IActionResult> OnGet(int skip, CancellationToken cancellationToken, int top = 20)
    {
        Skip = skip == -1 ? 0 : skip;
        Top = top;
        var userId = User?.FindFirst("sub")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var physicianId))
        {
            return RedirectToPage("/");
        }
        var result = await repository.GetSchedulesAsync(physicianId!, skip, null, top, cancellationToken);
        Schedules = result.ValueOrDefault ?? [];
        return Page();
    }
}
