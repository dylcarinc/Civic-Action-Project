namespace CivicAction.Models;

public class Verification
{
    public int Id { get; set; }
    public bool IsApproved { get; set; }
    public string Feedback { get; set; }
    public int AdminID { get; set; }
    public int ProjectID { get; set; }
}