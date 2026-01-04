using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Application.Interfaces.Repositories.IMedSchedRepository;

namespace WebUi.Pages.Schedule;

public class IndexModel(IMedSchedRepository repository) : PageModel
{
    public GetAllSchedulesDto Schedules { get; set; } = new GetAllSchedulesDto([], 0);

    [BindProperty(SupportsGet = true)]
    [Range(minimum: 0, maximum: int.MaxValue)]
    public int Skip { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Top { get; set; } = 10;
    public bool HasNext => (Skip + 1) * Top < Schedules?.Count;
    public async Task<IActionResult> OnGet(int skip, CancellationToken cancellationToken, int top = 10)
    {
        Skip = skip == -1 ? 0 : skip;
        Top = top;
        var userId = User?.FindFirst("sub")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var physicianId))
        {
            return RedirectToPage("/");
        }
        var result = await repository.GetSchedulesAsync(physicianId!, skip, null, top, cancellationToken);
        if (result.IsFailed || result.Value == null)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }
            return Page();
        }
        Schedules = result.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid physicianId, Guid scheduleId,CancellationToken cancellationToken)
    {
        var result = await repository.DeleteScheduleAsync(physicianId, scheduleId, cancellationToken);
        if (result.IsFailed)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }
            return Page();
        }
        return RedirectToPage(new { skip = Skip, top = Top });
    }
}
