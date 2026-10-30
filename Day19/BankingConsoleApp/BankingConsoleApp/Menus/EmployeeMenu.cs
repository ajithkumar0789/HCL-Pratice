
using BankingConsoleApp.Model;
using BankingConsoleApp.Services;

namespace BankingConsoleApp.Menus;

public class EmployeeMenu
{
    private readonly EmployeeService _service;

    public EmployeeMenu(EmployeeService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== EMPLOYEE MANAGEMENT =====");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View Employees");
            Console.WriteLine("3. Search Employee");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Back");

            switch (ConsoleInput.ReadText("Enter choice: "))
            {
                case "1": await AddAsync(); break;
                case "2": await ViewAsync(); break;
                case "3": await SearchAsync(); break;
                case "4": await UpdateAsync(); break;
                case "5": await DeleteAsync(); break;
                case "6": return;
                default: Console.WriteLine("Invalid choice."); break;
            }

            ConsoleInput.Pause();
        }
    }

    private async Task AddAsync()
    {
        var employee = new Employee
        {
            EmployeeName = ConsoleInput.ReadText("Employee name: "),
            Email = ConsoleInput.ReadText("Email: "),
            Phone = ConsoleInput.ReadText("Phone: "),
            Designation = ConsoleInput.ReadText("Designation: "),
            BranchId = ConsoleInput.ReadInt("Branch ID: ")
        };

        await _service.AddEmployeeAsync(employee);
        Console.WriteLine($"Employee added. ID: {employee.EmployeeId}");
    }

    private async Task ViewAsync()
    {
        var employees = await _service.GetAllEmployeesAsync();

        foreach (var employee in employees)
            Console.WriteLine($"{employee.EmployeeId} | {employee.EmployeeName} | {employee.Email} | {employee.Designation} | Branch: {employee.BranchId}");

        if (employees.Count == 0)
            Console.WriteLine("No employees found.");
    }

    private async Task SearchAsync()
    {
        int id = ConsoleInput.ReadInt("Employee ID: ");
        var employee = await _service.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        Console.WriteLine($"{employee.EmployeeId} | {employee.EmployeeName} | {employee.Email} | {employee.Phone} | {employee.Designation}");
    }

    private async Task UpdateAsync()
    {
        int id = ConsoleInput.ReadInt("Employee ID: ");
        var employee = await _service.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        employee.EmployeeName = ConsoleInput.ReadText("New name: ");
        employee.Email = ConsoleInput.ReadText("New email: ");
        employee.Phone = ConsoleInput.ReadText("New phone: ");
        employee.Designation = ConsoleInput.ReadText("New designation: ");
        employee.BranchId = ConsoleInput.ReadInt("New branch ID: ");

        await _service.UpdateEmployeeAsync(employee);
        Console.WriteLine("Employee updated.");
    }

    private async Task DeleteAsync()
    {
        int id = ConsoleInput.ReadInt("Employee ID: ");
        await _service.DeleteEmployeeAsync(id);
        Console.WriteLine("Delete request completed.");
    }
}
