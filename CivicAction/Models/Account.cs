using System.ComponentModel.DataAnnotations;

namespace CivicAction.Models;

public enum Grade
{
    Freshman, Sophomore, Junior, Senior
}

public class Account
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstMidName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Grade Grade { get; set; }
    public string School { get; set; }
    public bool IsAdmin { get; set; }
}