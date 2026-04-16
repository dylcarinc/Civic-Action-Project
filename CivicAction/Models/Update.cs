using Humanizer;

namespace CivicAction.Models;

public class Update
{
    public int Id { get; set; }
    public string Description { get; set; }
    public double HoursDone { get; set; }
    public int StudentID { get; set; }
    public int ProjectID { get; set; }
}