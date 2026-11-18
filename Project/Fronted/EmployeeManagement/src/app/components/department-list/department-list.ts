
import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';

import { FormsModule } from '@angular/forms';

import { DepartmentService } from '../../services/department';
import { Department } from '../../models/department';

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './department-list.html',
  styleUrl: './department-list.css'
})
export class DepartmentListComponent implements OnInit {

  // Store all departments
  departments: Department[] = [];

  message = '';
  errorMessage = '';

  // Edit department properties
  editingId: number | null = null;
  editDepartmentName = '';
  editDescription = '';

  constructor(
    private departmentService: DepartmentService,
    private cdr: ChangeDetectorRef
  ) {}

  // Load departments when component starts
  ngOnInit(): void {
    this.loadDepartments();
  }

  // Get all departments from API
  loadDepartments(): void {

    console.log('Calling Department API...');

    this.departmentService.getAllDepartments().subscribe({

      next: (data: Department[]) => {

        console.log('Department API Response:', data);

        // Assign API response
        this.departments = data ?? [];

        console.log('Departments Array:', this.departments);
        console.log('Total Departments:', this.departments.length);

        this.errorMessage = '';

        // Update the HTML view
        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Department API Error:', error);

        this.departments = [];
        this.errorMessage = 'Unable to load departments.';

        this.cdr.detectChanges();
      }

    });
  }

  // Start editing a department
  editDepartment(department: Department): void {

    this.editingId = department.id;
    this.editDepartmentName = department.departmentName;
    this.editDescription = department.description ?? '';

    this.message = '';
    this.errorMessage = '';
  }

  // Update department
  updateDepartment(): void {

    if (!this.editDepartmentName.trim()) {
      this.errorMessage = 'Department name is required.';
      return;
    }

    if (this.editingId === null) {
      return;
    }

    const department = {
      departmentName: this.editDepartmentName.trim(),
      description: this.editDescription.trim() || null
    };

    this.departmentService.updateDepartment(
      this.editingId,
      department
    ).subscribe({

      next: () => {

        this.message = 'Department updated successfully!';
        this.errorMessage = '';

        this.cancelEdit();
        this.loadDepartments();
      },

      error: (error) => {

        console.error('Update Error:', error);

        this.errorMessage =
          error.error?.message || 'Unable to update department.';

        this.message = '';
      }

    });
  }

  // Cancel editing
  cancelEdit(): void {

    this.editingId = null;
    this.editDepartmentName = '';
    this.editDescription = '';
  }

  // Delete department
  deleteDepartment(id: number): void {

    const confirmDelete = confirm(
      'Are you sure you want to delete this department?'
    );

    if (!confirmDelete) {
      return;
    }

    this.departmentService.deleteDepartment(id).subscribe({

      next: () => {

        this.message = 'Department deleted successfully!';
        this.errorMessage = '';

        this.loadDepartments();
      },

      error: (error) => {

        console.error('Delete Error:', error);

        this.errorMessage =
          error.error?.message || 'Unable to delete department.';

        this.message = '';
      }

    });
  } 

}
