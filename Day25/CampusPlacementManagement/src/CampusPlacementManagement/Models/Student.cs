
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models;

public class Student
{
    public int StudentId { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    public decimal CGPA { get; set; }

    public int CollegeId { get; set; }

    public College College { get; set; } = null!;
}
