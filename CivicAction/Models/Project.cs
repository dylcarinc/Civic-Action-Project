namespace CivicAction.Models;

public class Project
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstMidName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Grade Grade { get; set; }
    public string School { get; set; }
    public string Description { get; set; }
    public double Hours { get; set; }
    public string Organization { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public int StudentID { get; set; }
    public bool IsApproved { get; set; }
}