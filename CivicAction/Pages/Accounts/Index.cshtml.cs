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
    public class IndexModel : PageModel
    {
        private readonly CivicAction.Data.CivicActionContext _context;

        public IndexModel(CivicAction.Data.CivicActionContext context)
        {
            _context = context;
        }

        public IList<Account> Account { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

            if (!isAdmin)
            {
                return RedirectToPage("/Projects/Index");
            }

            var query = _context.Accounts
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
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstMidName)
                .ToListAsync();

            return Page();
        }
    }
}