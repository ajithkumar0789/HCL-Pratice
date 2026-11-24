
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  StudentResponse,
  UpdateStudentRequest,
} from '../../admin/students/student.models';

import { StudentService } from '../../admin/students/student.service';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './student-profile.html',
  styleUrl: './student-profile.css',
})
export class StudentProfile implements OnInit {
  private readonly studentService = inject(StudentService);

  profile: StudentResponse | null = null;

  form: UpdateStudentRequest = {
    name: '',
    email: '',
    department: '',
    age: 0,
    cgpa: 0,
  };

  isLoading = true;
  isEditing = false;
  isSaving = false;

  errorMessage = '';
  successMessage = '';

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.studentService.getMyProfile().subscribe({
      next: (response) => {
        this.profile = response;
        this.copyProfileToForm();
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.detail ||
          error.error?.message ||
          'Unable to load your profile.';
        this.isLoading = false;
      },
    });
  }

  editProfile(): void {
    this.copyProfileToForm();
    this.isEditing = true;
    this.successMessage = '';
    this.errorMessage = '';
  }

  cancelEdit(): void {
    this.copyProfileToForm();
    this.isEditing = false;
  }

  saveProfile(): void {
    if (!this.profile || this.isSaving) return;

    this.isSaving = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.studentService.updateMyProfile(this.form).subscribe({
      next: (response) => {
        this.profile = response;
        this.copyProfileToForm();
        this.isEditing = false;
        this.isSaving = false;
        this.successMessage = 'Profile updated successfully.';
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.detail ||
          error.error?.message ||
          'Unable to update your profile.';
        this.isSaving = false;
      },
    });
  }

  private copyProfileToForm(): void {
    if (!this.profile) return;

    this.form = {
      name: this.profile.name,
      email: this.profile.email,
      department: this.profile.department,
      age: this.profile.age,
      cgpa: this.profile.cgpa,
    };
  }

  statusLabel(status: number): string {
    switch (status) {
      case 1:
        return 'Approved';
      case 2:
        return 'Rejected';
      default:
        return 'Pending';
    }
  }
}
