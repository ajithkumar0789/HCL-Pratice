

using System;
using System.Collections.Generic;

// Custom delegate
delegate void PayrollNotification(string message);

class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Deduction { get; set; }

    public Employee(int id, string name, decimal basicSalary,
                    decimal allowance, decimal deduction)
    {
        Id = id;
        Name = name;
        BasicSalary = basicSalary;
        Allowance = allowance;
        Deduction = deduction;
    }
}

class Program
{
    // Multicast delegate methods
    static void DisplayNotification(string message)
    {
        Console.WriteLine("Notification: " + message);
    }

    static void LogNotification(string message)
    {
        Console.WriteLine("Log: Payroll processed - " + message);
    }

    // Salary calculation methods
    static decimal CalculateStandardSalary(Employee e)
    {
        return e.BasicSalary + e.Allowance - e.Deduction;
    }

    static decimal CalculateBonusSalary(Employee e)
    {
        decimal bonus = e.BasicSalary * 0.10m;

        return e.BasicSalary + e.Allowance
               + bonus - e.Deduction;
    }

    static void Main()
    {
        List<Employee> employees = new List<Employee>
        {
            new Employee(101, "Arun", 30000, 5000, 2000),
            new Employee(102, "Priya", 40000, 6000, 3000),
            new Employee(103, "Kumar", 25000, 3000, 1000)
        };

        // Action delegate: display messages
        Action<string> print = Console.WriteLine;

        // Custom multicast delegate
        PayrollNotification notify = DisplayNotification;
        notify += LogNotification;

        while (true)
        {
            print("\n===== PAYROLL MANAGEMENT =====");
            print("1. Display Employees");
            print("2. Calculate Employee Salary");
            print("3. Find High-Paid Employees");
            print("4. Exit");
            print("Enter choice:");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                print("Invalid input. Enter a number.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        foreach (Employee e in employees)
                        {
                            print($"ID: {e.Id}, Name: {e.Name}, " +
                                  $"Basic Salary: {e.BasicSalary:C}");
                        }
                        break;

                    case 2:
                        print("Enter Employee ID:");

                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            print("Invalid employee ID.");
                            break;
                        }

                        Employee? employee =
                            employees.Find(e => e.Id == id);

                        if (employee == null)
                        {
                            print("Employee not found.");
                            break;
                        }

                        print("1. Standard Salary");
                        print("2. Salary with 10% Bonus");

                        if (!int.TryParse(Console.ReadLine(),
                                          out int salaryChoice))
                        {
                            print("Invalid choice.");
                            break;
                        }

                        // Func delegate references a method
                        Func<Employee, decimal> calculator;

                        if (salaryChoice == 1)
                        {
                            calculator = CalculateStandardSalary;
                        }
                        else if (salaryChoice == 2)
                        {
                            calculator = CalculateBonusSalary;
                        }
                        else
                        {
                            print("Invalid salary option.");
                            break;
                        }

                        decimal netSalary = calculator(employee);

                        print("\n===== PAYSLIP =====");
                        print($"Employee: {employee.Name}");
                        print($"Basic Salary: {employee.BasicSalary:C}");
                        print($"Allowance: {employee.Allowance:C}");
                        print($"Deduction: {employee.Deduction:C}");
                        print($"Net Salary: {netSalary:C}");

                        // Invoke both notification methods
                        notify(employee.Name);

                        break;

                    case 3:
                        print("Enter minimum net salary:");

                        if (!decimal.TryParse(Console.ReadLine(),
                                              out decimal minimum))
                        {
                            print("Enter a valid salary amount.");
                            break;
                        }

                        // Predicate delegate checks a condition
                        Predicate<Employee> isHighPaid =
                            e => CalculateStandardSalary(e) >= minimum;

                        print("\nHigh-Paid Employees:");

                        bool found = false;

                        foreach (Employee e in employees)
                        {
                            if (isHighPaid(e))
                            {
                                print($"{e.Name} - " +
                                      $"{CalculateStandardSalary(e):C}");
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            print("No matching employees found.");
                        }

                        break;

                    case 4:
                        print("Exiting Payroll System...");
                        return;

                    default:
                        print("Choose a valid option from 1 to 4.");
                        break;
                }
            }
            catch (Exception ex)
            {
                print("Unexpected error: " + ex.Message);
            }
        }
    }
}