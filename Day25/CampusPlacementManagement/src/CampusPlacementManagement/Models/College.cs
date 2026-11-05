
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models;

public class College
{
    public int CollegeId { get; set; }

    [Required]
    [MaxLength(150)]
    public string CollegeName { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? ContactEmail { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Student> Students { get; set; } = new List<Student>();
}
