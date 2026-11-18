
import { Component, OnInit } from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';

import { EmployeeService } from '../../services/employee';
import { DepartmentService } from '../../services/department';
import { Department } from '../../models/department';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-employee.html',
  styleUrl: './add-employee.css'
})
export class AddEmployeeComponent implements OnInit {

  employeeForm: FormGroup;

  departments: Department[] = [];

  message = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService
  ) {

    this.employeeForm = this.fb.group({

      name: ['', Validators.required],

      email: ['', [
        Validators.required,
        Validators.email
      ]],

      phone: ['', [
        Validators.required,
        Validators.pattern(/^9[0-9]{9}$/)
      ]],

      departmentId: ['', Validators.required],

      designation: ['', Validators.required],

      salary: ['', [
        Validators.required,
        Validators.min(0.01)
      ]]

    });
  }

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {

    this.departmentService.getAllDepartments().subscribe({

      next: (data: Department[]) => {
        this.departments = data ?? [];
        this.errorMessage = '';

        if (this.departments.length === 0) {
          this.errorMessage = 'No departments available. Please add a department first.';
        }
      },

      error: (error) => {
        console.error('Department loading error:', error);
        this.errorMessage = 'Unable to load departments.';
      }

    });
  }

  onSubmit(): void {

    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    this.employeeService
      .addEmployee(this.employeeForm.value)
      .subscribe({

        next: (response) => {

          this.message = 'Employee added successfully!';
          this.errorMessage = '';

          console.log(response);

          this.employeeForm.reset();

        },

        error: (error) => {

          this.message = '';
          this.errorMessage = 'Failed to add employee';

          console.error(error);

        }

      });

  }

}
