using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.DTOs.Department
{
    public class CreateDepartmentDto
    {
        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }
    }
}