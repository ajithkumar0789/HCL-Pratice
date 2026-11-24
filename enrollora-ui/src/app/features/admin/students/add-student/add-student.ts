
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Router } from '@angular/router';

import { StudentService } from '../student.service';

import { RegisterStudentRequest } from '../../../../core/auth/auth.models';

@Component({
  selector: 'app-add-student',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './add-student.html',
  styleUrl: './add-student.css'
})
export class AddStudent {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private studentService = inject(StudentService);

  studentForm: FormGroup = this.fb.group({
    name: ['', Validators.required],

    email: ['', [
      Validators.required,
      Validators.email
    ]],

    password: ['', [
      Validators.required,
      Validators.minLength(8)
    ]],

    department: ['', Validators.required],

    age: ['', [
      Validators.required,
      Validators.min(17),
      Validators.max(60)
    ]],

    cgpa: ['', [
      Validators.required,
      Validators.min(0),
      Validators.max(10)
    ]]
  });

  successMessage = '';
  errorMessage = '';
  isSubmitting = false;

  departments = [
    'CSE',
    'ECE',
    'EEE',
    'IT',
    'Mechanical'
  ];

  onSubmit(): void {

    if (this.studentForm.invalid || this.isSubmitting) {
      this.studentForm.markAllAsTouched();
      return;
    }

    const studentData: RegisterStudentRequest = {
      name: this.studentForm.value.name.trim(),
      email: this.studentForm.value.email.trim(),
      password: this.studentForm.value.password,
      department: this.studentForm.value.department,
      age: Number(this.studentForm.value.age),
      cgpa: Number(this.studentForm.value.cgpa)
    };

    this.isSubmitting = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.studentService.addStudent(studentData).subscribe({

      next: (response) => {

        console.log('Student registered:', response);

        this.isSubmitting = false;

        this.successMessage =
          response.message || 'Student registered successfully!';

        this.studentForm.reset();

        setTimeout(() => {
          this.router.navigate(['/admin/students']);
        }, 1500);

      },

      error: (error) => {

        console.error('Registration failed:', error);

        this.isSubmitting = false;

        this.errorMessage =
          error.error?.detail ||
          error.error?.message ||
          'Unable to register student. Please try again.';

      }

    });
  }

  resetForm(): void {
    this.studentForm.reset();
    this.successMessage = '';
    this.errorMessage = '';
  }

  goBack(): void {
    this.router.navigate(['/admin/students']);
  }
}
