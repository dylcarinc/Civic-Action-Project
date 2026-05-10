using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CivicAction.Pages.Projects;

[Authorize]
public class DetailsModel(CivicActionContext context, UserManager<AppUser> userManager) : PageModel
{
    public Project Project { get; set; } = default!;
    public List<Update> Updates { get; set; } = new();

    [BindProperty]
    public Update NewUpdate { get; set; } = new();

    [BindProperty]
    public string ReviewFeedback { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var project = await context.Projects
            .Include(p => p.Updates)
            .Include(p => p.Student)
            .Include(p => p.Verifications)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (project is null) return NotFound();

        Project = project;
        Updates = project.Updates.ToList();
        NewUpdate.Date = DateOnly.FromDateTime(DateTime.Now);
        return Page();
    }

   public async Task<IActionResult> OnPostAsync(int? id)
{
    if (id == null) return NotFound();

    ModelState.Remove("ReviewFeedback");
    ModelState.Remove("NewUpdate.StudentID");

    var project = await context.Projects
        .Include(p => p.Updates)
        .Include(p => p.Student)
        .Include(p => p.Verifications)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (project is null) return NotFound();

    if (!ModelState.IsValid)
    {
        Project = project;
        Updates = project.Updates.ToList();
        return Page();
    }

    try
    {
        var user = await userManager.GetUserAsync(User);
        NewUpdate.ProjectID = project.Id;
        NewUpdate.StudentID = user?.Id ?? project.StudentID;
        var startDateTime = new DateTime(NewUpdate.Date.Year, NewUpdate.Date.Month, NewUpdate.Date.Day, NewUpdate.StartTime.Hour, NewUpdate.StartTime.Minute, NewUpdate.StartTime.Second);
        var endDateTime = new DateTime(NewUpdate.Date.Year, NewUpdate.Date.Month, NewUpdate.Date.Day, NewUpdate.EndTime.Hour, NewUpdate.EndTime.Minute, NewUpdate.EndTime.Second);
        if (endDateTime <= startDateTime)
        {
            endDateTime = endDateTime.AddDays(1);
        }
        var timeSpan = endDateTime - startDateTime;
        NewUpdate.HoursDone = timeSpan.TotalHours;
        if (NewUpdate.HoursDone <= 0)
        {
            ModelState.AddModelError("", "End time must be after start time.");
            Project = project;
            Updates = project.Updates.ToList();
            return Page();
        }
        context.Updates.Add(NewUpdate);
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", $"Error adding update: {ex.Message}");
        Project = project;
        Updates = project.Updates.ToList();
        return Page();
    }

    return RedirectToPage(new { id });
}

    public async Task<IActionResult> OnPostReviewAsync(int? id, string decision)
    {
        ModelState.Remove("NewUpdate.Description");
        ModelState.Remove("NewUpdate.HoursDone");
        ModelState.Remove("ReviewFeedback");

        if (id == null) return NotFound();

        var user = await userManager.GetUserAsync(User);
        if (user == null || !user.IsAdmin)
            return Forbid();

        var project = await context.Projects
            .Include(p => p.Verifications)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (project is null) return NotFound();

        project.IsApproved = decision == "approve";

        var verification = new Verification
        {
            ProjectID = project.Id,
            AdminID = user.Id,
            IsApproved = project.IsApproved,
            Feedback = ReviewFeedback
        };

        context.Verifications.Add(verification);
        await context.SaveChangesAsync();

        return RedirectToPage(new { id });
    }
}