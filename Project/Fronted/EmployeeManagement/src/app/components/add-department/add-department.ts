import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DepartmentService } from '../../services/department';

@Component({
  selector: 'app-add-department',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-department.html',
  styleUrl: './add-department.css'
})
export class AddDepartmentComponent {

  departmentName = '';
  description = '';
  message = '';
  errorMessage = '';

  constructor(
    private departmentService: DepartmentService,
    private router: Router
  ) {}

  onSubmit(): void {

    if (!this.departmentName.trim()) {
      this.errorMessage = 'Department name is required.';
      return;
    }

    const department = {
      departmentName: this.departmentName.trim(),
      description: this.description.trim() || null
    };

    this.departmentService.addDepartment(department).subscribe({
      next: () => {
        this.message = 'Department added successfully!';
        this.errorMessage = '';

        this.departmentName = '';
        this.description = '';

        this.router.navigate(['/departments-list']);
      },
      error: (error) => {
        this.message = '';

        this.errorMessage =
          error.error?.message || 'Unable to add department.';
      }
    });
  }
}