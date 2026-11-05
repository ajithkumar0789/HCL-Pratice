namespace CampusPlacementManagement.DTOs.Students;

public class StudentResponse
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Department { get; set; } = string.Empty;
    public decimal CGPA { get; set; }
    public int CollegeId { get; set; }
    public string CollegeName { get; set; } = string.Empty;
}