
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.DTOs
{
    public class UpdateSalaryDto
    {
        [Range(0.01, 999999999)]
        public decimal Salary { get; set; }
    }
}
