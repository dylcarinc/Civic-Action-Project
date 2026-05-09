using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CivicAction.Data;
using CivicAction.Models;

namespace CivicAction.Pages.Verifications
{
    public class CreateModel : PageModel
    {
        private readonly CivicAction.Data.CivicActionContext _context;

        public CreateModel(CivicAction.Data.CivicActionContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AdminID"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Verification Verification { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Verifications.Add(Verification);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
