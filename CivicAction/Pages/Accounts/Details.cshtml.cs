using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CivicAction.Data;
using CivicAction.Models;

namespace CivicAction.Pages.Accounts
{
    public class DetailsModel : PageModel
    {
        private readonly CivicAction.Data.CivicActionContext _context;

        public DetailsModel(CivicAction.Data.CivicActionContext context)
        {
            _context = context;
        }

        public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

            if (!isAdmin)
            {
                return RedirectToPage("/Projects/Index");
            }

            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Projects)
                    .ThenInclude(p => p.Updates)
                .Include(a => a.Projects)
                    .ThenInclude(p => p.Verifications)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsAdmin);

            if (account is not null)
            {
                Account = account;
                return Page();
            }

            return NotFound();
        }
    }
}