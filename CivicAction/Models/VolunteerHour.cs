using System.ComponentModel.DataAnnotations;

namespace CivicAction.Models;

public class VolunteerHour
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Date is required")]
    public DateTime WorkDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Hours are required")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Hours must be greater than 0")]
    public double Hours { get; set; }

    [Required(ErrorMessage = "Work description is required")]
    public string WorkDescription { get; set; } = string.Empty;

    public int VolunteerOrganizationID { get; set; }
    public VolunteerOrganization? VolunteerOrganization { get; set; }
}