
import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../services/employee';
import { Employee } from '../../models/employee';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css'
})
export class EmployeeListComponent implements OnInit {

  employees: Employee[] = [];
  searchId: number | null = null;
  searchedEmployee: Employee | null = null;

  message = '';

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  // Get all employees
  loadEmployees(): void {
    this.employeeService.getAllEmployees().subscribe({
      next: (data) => {
        this.employees = data;
      },
      error: () => {
        this.message = 'Unable to load employees.';
      }
    });
  }

  // Search employee by ID
  searchEmployee(): void {

    if (this.searchId === null || this.searchId <= 0) {
      this.message = 'Please enter a valid employee ID.';
      this.searchedEmployee = null;
      return;
    }

    this.message = '';

    this.employeeService.getEmployeeById(this.searchId).subscribe({
      next: (data) => {
        this.searchedEmployee = data;
        this.message = '';
      },
      error: () => {
        this.searchedEmployee = null;
        this.message = 'Employee not found.';
      }
    });
  }

  // Clear search
  clearSearch(): void {
    this.searchId = null;
    this.searchedEmployee = null;
    this.message = '';
  }
}
