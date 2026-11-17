import { Component } from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';

import { EmployeeService } from '../../services/employee';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-employee.html',
  styleUrl: './add-employee.css'
})
export class AddEmployeeComponent {

  employeeForm: FormGroup;

  message = '';

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService
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

      department: ['', Validators.required],

      designation: ['', Validators.required],

      salary: ['', [
        Validators.required,
        Validators.min(0.01)
      ]]

    });

  }

  onSubmit() {

    if (this.employeeForm.invalid) {

      this.employeeForm.markAllAsTouched();

      return;

    }

    this.employeeService
      .addEmployee(this.employeeForm.value)
      .subscribe({

        next: (response) => {

          this.message = 'Employee added successfully!';

          console.log(response);

          this.employeeForm.reset();

        },

        error: (error) => {

          this.message = 'Failed to add employee';

          console.error(error);

        }

      });

  }

}