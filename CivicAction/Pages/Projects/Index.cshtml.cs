using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CivicAction.Data;
using CivicAction.Models;

namespace CivicAction.Pages.Projects
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CivicAction.Data.CivicActionContext _context;

        public IndexModel(CivicAction.Data.CivicActionContext context)
        {
            _context = context;
        }

        public IList<Project> Project { get;set; } = new List<Project>();
        public CivicAction.Models.Account UserAccount { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return;

            // Get current user's Account
            UserAccount = await _context.Accounts
                .Include(a => a.IdentityUser)
                .FirstOrDefaultAsync(a => a.IdentityUserId == userId);

            if (UserAccount == null)
                return;

            // Load only this user's projects
            Project = await _context.Projects
                .Where(p => p.StudentID == UserAccount.Id)
                .Include(p => p.Student)
                .ToListAsync();
        }
    }
}
