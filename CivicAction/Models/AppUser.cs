using Microsoft.AspNetCore.Identity;

namespace CivicAction.Models;

public enum Grade
{
    Freshman, Sophomore, Junior, Senior
}

public class AppUser : IdentityUser
{
    public string LastName { get; set; } = string.Empty;
    public string FirstMidName { get; set; } = string.Empty;
    public Grade Grade { get; set; }
    public string School { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}