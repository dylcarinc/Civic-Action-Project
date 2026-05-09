using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CivicAction.Pages.Projects;

[Authorize]
public class IndexModel(CivicActionContext context, UserManager<AppUser> userManager) : PageModel
{
    public IList<Project> Projects { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToPage("/Account/Login");

        Projects = user.IsAdmin
            ? await context.Projects
                .Include(p => p.Student)
                .Include(p => p.Verifications)
                .ToListAsync()
            : await context.Projects
                .Include(p => p.Student)
                .Include(p => p.Verifications)
                .Where(p => p.StudentID == user.Id)
                .ToListAsync();

        return Page();
    }
}