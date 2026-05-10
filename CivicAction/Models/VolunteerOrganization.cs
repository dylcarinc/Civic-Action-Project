using System.ComponentModel.DataAnnotations;

namespace CivicAction.Models;

public class VolunteerOrganization
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Organization name is required")]
    public string Name { get; set; } = string.Empty;

    public string StudentID { get; set; } = string.Empty;
    public AppUser? Student { get; set; }

    public ICollection<VolunteerHour> VolunteerHours { get; set; } = new List<VolunteerHour>();
}