using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebUi.Pages.Schedule;

public class NewSlotModel(IMedSchedRepository repository) : PageModel
{
    [BindProperty]
    [Required, DataType(DataType.DateTime)]
    public required DateTime StartDate { get; set; }
    [BindProperty]
    [Required, DataType(DataType.Time)]
    public required TimeOnly EndDate { get; set; }
    [BindProperty]
    [Required]
    public bool RepeatWeekly { get; set; }
    [BindProperty]
    [Range(1, 52)]
    public int? WeeksToRepeat { get; set; }
    [BindProperty]
    public List<int> SelectedDays { get; set; } = [];
    public void OnGet()
    {
        var initialStartDate = DateTime.Now.AddHours(1);
        var date = new DateOnly(initialStartDate.Year, initialStartDate.Month, initialStartDate.Day);
        var time = new TimeOnly(initialStartDate.Hour, initialStartDate.Minute);
        StartDate = new DateTime(date, time, DateTimeKind.Local);
    }
    public async Task<IActionResult> OnPost()
    {
        var endDate = new DateTime(
            StartDate.Year,
            StartDate.Month,
            StartDate.Day,
            EndDate.Hour,
            EndDate.Minute,
            0);
        if (StartDate >= endDate)
        {
            ModelState.AddModelError(nameof(EndDate), "A data final deve ser depois da data de início.");
        }
        if (StartDate < DateTime.Now)
        {
            ModelState.AddModelError(nameof(StartDate), "A data de início deve estar no futuro.");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var range = new DateRange(StartDate, endDate);
        var days = SelectedDays.Select(d => (DayOfWeek)d).ToList();

        var avalilableSlot = new AvailableSlot(range, RepeatWeekly, WeeksToRepeat ?? 0, days);
        var slots = avalilableSlot.GetAvailableSlots();

        var physicianId = User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(physicianId, out var userId))
        {
            return Unauthorized();
        }
        var result = await repository.CreateAvailableSlotAsync(slots.Select(s => s.Range), userId, CancellationToken.None);
        if (result.IsFailed)
        {
            ModelState.AddModelError(string.Empty, result.Errors[0]!.Message);
            return Page();
        }
        return RedirectToPage("/Schedule/Index");
    }
}
