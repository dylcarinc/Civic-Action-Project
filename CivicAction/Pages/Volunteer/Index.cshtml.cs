using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CivicAction.Pages.Volunteer;

[Authorize]
public class IndexModel(CivicActionContext context, UserManager<AppUser> userManager) : PageModel
{
    public List<VolunteerOrganization> Organizations { get; set; } = new();
    public double TotalHours { get; set; }

    [BindProperty]
    public string NewOrganizationName { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToPage("/Index");
        }

        if (user.IsAdmin)
        {
            return RedirectToPage("/Projects/Index");
        }

        await LoadPageData(user.Id);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToPage("/Index");
        }

        if (user.IsAdmin)
        {
            return RedirectToPage("/Projects/Index");
        }

        if (string.IsNullOrWhiteSpace(NewOrganizationName))
        {
            ModelState.AddModelError("NewOrganizationName", "Organization name is required");
            await LoadPageData(user.Id);
            return Page();
        }

        var organization = new VolunteerOrganization
        {
            Name = NewOrganizationName,
            StudentID = user.Id
        };

        context.VolunteerOrganizations.Add(organization);
        await context.SaveChangesAsync();

        return RedirectToPage("/Volunteer/Details", new { id = organization.Id });
    }

    private async Task LoadPageData(string studentId)
    {
        Organizations = await context.VolunteerOrganizations
            .Include(o => o.VolunteerHours)
            .Where(o => o.StudentID == studentId)
            .OrderBy(o => o.Name)
            .ToListAsync();

        TotalHours = Organizations
            .SelectMany(o => o.VolunteerHours)
            .Sum(h => h.Hours);
    }
}