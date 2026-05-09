using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CivicAction.Pages.Accounts;

[Authorize]
public class EditModel(UserManager<AppUser> userManager) : PageModel
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var account = await userManager.FindByIdAsync(Account.Id);
        if (account is null) return NotFound();

        account.FirstMidName = Account.FirstMidName;
        account.LastName = Account.LastName;
        account.Grade = Account.Grade;
        account.School = Account.School;
        account.IsAdmin = Account.IsAdmin;
        account.Email = Account.Email;
        account.UserName = Account.Email;

        var result = await userManager.UpdateAsync(account);

        if (result.Succeeded)
            return RedirectToPage("./Index");

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return Page();
    }
}