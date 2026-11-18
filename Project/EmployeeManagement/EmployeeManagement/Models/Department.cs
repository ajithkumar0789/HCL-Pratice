
namespace EmployeeManagement.API.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; }

        // One Department can have many Employees
        public ICollection<Employee> Employees { get; set; }
            = new List<Employee>();
    }
}
