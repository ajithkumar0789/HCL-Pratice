
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.DTOs
{
    public class CreateEmployeeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        // Foreign Key
        [Required]
        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        [Required]
        public string Designation { get; set; } = string.Empty;

        [Range(0.01, 999999999)]
        public decimal Salary { get; set; }
    }
}
