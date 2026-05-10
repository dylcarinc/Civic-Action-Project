using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CivicAction.Pages.Accounts;

[Authorize]
public class DetailsModel(CivicActionContext context, UserManager<AppUser> userManager) : PageModel
{
    public AppUser Account { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null || !user.IsAdmin)
        {
            return RedirectToPage("/Projects/Index");
        }

        if (id == null)
        {
            return NotFound();
        }

        var account = await context.Users
            .Include(a => a.Projects)
                .ThenInclude(p => p.Verifications)
            .Include(a => a.VolunteerOrganizations)
                .ThenInclude(o => o.VolunteerHours)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsAdmin);

        if (account is null)
        {
            return NotFound();
        }

        Account = account;
        return Page();
    }
}