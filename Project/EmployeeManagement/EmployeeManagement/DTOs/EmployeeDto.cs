
namespace EmployeeManagement.API.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public decimal Salary { get; set; }
    }
}
