using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CivicAction.Pages.Volunteer;

[Authorize]
public class DetailsModel(CivicActionContext context, UserManager<AppUser> userManager) : PageModel
{
    public VolunteerOrganization Organization { get; set; } = default!;

    [BindProperty]
    public VolunteerHour NewHour { get; set; } = new() { WorkDate = DateTime.Today };

    public async Task<IActionResult> OnGetAsync(int? id)
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

        if (id == null)
        {
            return NotFound();
        }

        var organization = await context.VolunteerOrganizations
            .Include(o => o.VolunteerHours)
            .FirstOrDefaultAsync(o => o.Id == id && o.StudentID == user.Id);

        if (organization == null)
        {
            return NotFound();
        }

        Organization = organization;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
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

        if (id == null)
        {
            return NotFound();
        }

        var organization = await context.VolunteerOrganizations
            .Include(o => o.VolunteerHours)
            .FirstOrDefaultAsync(o => o.Id == id && o.StudentID == user.Id);

        if (organization == null)
        {
            return NotFound();
        }

        ModelState.Remove("NewHour.VolunteerOrganizationID");
        ModelState.Remove("NewHour.VolunteerOrganization");

        if (!ModelState.IsValid)
        {
            Organization = organization;
            return Page();
        }

        NewHour.VolunteerOrganizationID = organization.Id;

        context.VolunteerHours.Add(NewHour);
        await context.SaveChangesAsync();

        return RedirectToPage("./Details", new { id = organization.Id });
    }
}