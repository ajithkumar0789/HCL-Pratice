using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Colleges;

public class CreateCollegeRequest
{
    [Required]
    [MaxLength(150)]
    public string CollegeName { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Address { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? ContactEmail { get; set; }
}