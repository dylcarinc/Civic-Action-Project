using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CivicAction.Pages.Accounts;

[Authorize]
public class DeleteModel(UserManager<AppUser> userManager) : PageModel
{
    [BindProperty]
    public AppUser Account { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id == null) return NotFound();

        var account = await userManager.FindByIdAsync(id);
        if (account is null) return NotFound();

        Account = account;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id == null) return NotFound();

        var account = await userManager.FindByIdAsync(id);
        if (account != null)
            await userManager.DeleteAsync(account);

        return RedirectToPage("./Index");
    }
}