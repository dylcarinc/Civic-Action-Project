namespace CivicAction.Models;

using System.ComponentModel.DataAnnotations;

public class Update
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Hours done is required")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Hours must be greater than 0")]
    public double HoursDone { get; set; }
    public bool IsWorkshop { get; set; }
    
    public string StudentID { get; set; } = string.Empty;
    public int ProjectID { get; set; }

    public Project? Project { get; set; }
}