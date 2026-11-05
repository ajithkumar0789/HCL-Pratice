using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Students;

public class CreateStudentRequest
{
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

    [Range(typeof(decimal), "0", "10")]
    public decimal CGPA { get; set; }

    [Range(1, int.MaxValue)]
    public int CollegeId { get; set; }
}