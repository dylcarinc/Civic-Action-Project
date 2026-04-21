using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CivicAction.Models;

public enum Grade
{
    Freshman, Sophomore, Junior, Senior
}

public class Account
{
    public int Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstMidName { get; set; } = string.Empty;
    public Grade Grade { get; set; }
    public string School { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    
    // Foreign key to IdentityUser
    public string IdentityUserId { get; set; } = string.Empty;
    public IdentityUser? IdentityUser { get; set; }
    
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}