
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { EmployeeService } from '../../services/employee';
import { Employee } from '../../models/employee';

import { DepartmentService } from '../../services/department';
import { Department } from '../../models/department';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css'
})
export class EmployeeListComponent implements OnInit {

  employees: Employee[] = [];
  departments: Department[] = [];

  searchId: number | null = null;
  searchedEmployee: Employee | null = null;

  message = '';

  // Employee editing
  editingEmployee: Employee | null = null;

  // Salary editing
  editingSalaryEmployeeId: number | null = null;
  updatedSalary: number | null = null;

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
    this.loadDepartments();
  }

  // Load all employees
  loadEmployees(): void {
    this.employeeService.getAllEmployees().subscribe({
      next: (data) => {
        this.employees = data;
      },
      error: (error) => {
        this.message = 'Unable to load employees.';
        console.error(error);
      }
    });
  }

  // Load departments
  loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (data) => {
        this.departments = data;
      },
      error: (error) => {
        this.message = 'Unable to load departments.';
        console.error(error);
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

  // Start editing employee details
  editEmployee(employee: Employee): void {

    this.editingSalaryEmployeeId = null;
    this.updatedSalary = null;

    this.editingEmployee = {
      ...employee
    };

    this.message = '';
  }

  // Cancel employee editing
  cancelEmployeeEdit(): void {
    this.editingEmployee = null;
    this.message = '';
  }

  // Save employee details
  saveEmployee(): void {

    if (this.editingEmployee === null) {
      return;
    }

    const employee = this.editingEmployee;

    // Check employee ID
    if (employee.id === undefined) {
      this.message = 'Employee ID is missing.';
      return;
    }

    // Validate employee details
    if (
      !employee.name?.trim() ||
      !employee.email?.trim() ||
      !employee.phone?.trim() ||
      !employee.designation?.trim() ||
      !employee.departmentId ||
      employee.salary <= 0
    ) {
      this.message = 'Please enter all valid employee details.';
      return;
    }

    this.employeeService.updateEmployee(
      employee.id,
      employee
    ).subscribe({
      next: (updatedEmployee) => {

        this.message = 'Employee updated successfully!';

        this.editingEmployee = null;

        this.loadEmployees();

        if (this.searchId === updatedEmployee.id) {
          this.searchEmployee();
        }
      },
      error: (error) => {
        this.message = 'Failed to update employee.';
        console.error(error);
      }
    });
  }

  // Delete employee
  deleteEmployee(id: number): void {

    if (!confirm('Are you sure you want to delete this employee?')) {
      return;
    }

    this.employeeService.deleteEmployee(id).subscribe({
      next: () => {

        this.message = 'Employee deleted successfully!';

        if (this.searchedEmployee?.id === id) {
          this.searchedEmployee = null;
          this.searchId = null;
        }

        if (this.editingEmployee?.id === id) {
          this.editingEmployee = null;
        }

        this.loadEmployees();
      },
      error: (error) => {
        this.message = 'Failed to delete employee.';
        console.error(error);
      }
    });
  }

  // Start editing salary
  editSalary(employee: Employee): void {

    if (employee.id === undefined) {
      this.message = 'Employee ID is missing.';
      return;
    }

    this.editingEmployee = null;
    this.editingSalaryEmployeeId = employee.id;
    this.updatedSalary = employee.salary;

    this.message = '';
  }

  // Cancel salary editing
  cancelSalary(): void {
    this.editingSalaryEmployeeId = null;
    this.updatedSalary = null;
    this.message = '';
  }

  // Save updated salary
  saveSalary(id: number): void {

    if (
      this.updatedSalary === null ||
      this.updatedSalary <= 0
    ) {
      this.message = 'Salary must be greater than zero.';
      return;
    }

    this.employeeService.updateSalary(
      id,
      this.updatedSalary
    ).subscribe({
      next: () => {

        this.message = 'Salary updated successfully!';

        this.cancelSalary();
        this.loadEmployees();

        if (this.searchedEmployee?.id === id) {
          this.searchEmployee();
        }
      },
      error: (error) => {
        this.message = 'Failed to update salary.';
        console.error(error);
      }
    });
  }
}
