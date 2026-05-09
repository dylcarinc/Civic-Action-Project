using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CivicAction.Pages.Accounts;

[Authorize]
public class IndexModel(CivicActionContext context, UserManager<AppUser> userManager) : PageModel
{
    public IList<AppUser> Account { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null || !user.IsAdmin)
                return RedirectToPage("/Projects/Index");

            var query = context.Users
                .Include(a => a.Projects)
                    .ThenInclude(p => p.Updates)
                .Where(a => !a.IsAdmin);

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(a =>
                    a.FirstMidName.Contains(SearchTerm) ||
                    a.LastName.Contains(SearchTerm));
            }

            Account = await query
                .OrderBy(a => a.LastName ?? "")
                .ThenBy(a => a.FirstMidName ?? "")
                .ToListAsync();

            return Page();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Accounts Index Error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            throw;
        }
    }
}