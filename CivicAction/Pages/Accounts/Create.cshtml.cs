using CivicAction.Data;
using CivicAction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CivicAction.Pages.Accounts;

[Authorize]
public class CreateModel(UserManager<AppUser> userManager) : PageModel
{
    [BindProperty]
    public AppUser Account { get; set; } = default!;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var result = await userManager.CreateAsync(Account, Password);

        if (result.Succeeded)
            return RedirectToPage("./Index");

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return Page();
    }
}