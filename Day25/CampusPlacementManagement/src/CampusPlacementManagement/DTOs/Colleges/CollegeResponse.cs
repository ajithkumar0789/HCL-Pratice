namespace CampusPlacementManagement.DTOs.Colleges;

public class CollegeResponse
{
    public int CollegeId { get; set; }
    public string CollegeName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? ContactEmail { get; set; }
    public bool IsActive { get; set; }
}