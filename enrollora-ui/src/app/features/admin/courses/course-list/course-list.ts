
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CourseService } from '../course.service';
import { CourseResponse, SaveCourseRequest } from '../course.models';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './course-list.html',
  styleUrl: './course-list.css',
})
export class CourseListComponent implements OnInit {

  private courseService = inject(CourseService);

  // Course Data
  courses = signal<CourseResponse[]>([]);

  isLoading = signal(false);
  errorMessage = signal('');

  // Pagination
  pageNumber = signal(1);
  pageSize = signal(10);

  totalRecords = signal(0);
  totalPages = signal(0);

  // Add Course Modal
  showFormModal = signal(false);
  isSubmitting = signal(false);
  formError = signal('');

  form: SaveCourseRequest = this.blankForm();

  private blankForm(): SaveCourseRequest {
    return {
      courseName: '',
      duration: '',
      fees: 0,
      capacity: 0,
      isActive: true
    };
  }

  ngOnInit(): void {
    this.loadCourses();
  }

  // Load Courses
  loadCourses(): void {

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.courseService.getCourses(
      this.pageNumber(),
      this.pageSize()
    ).subscribe({

      next: (response) => {

        this.courses.set(response.items);

        this.totalRecords.set(response.totalRecords);
        this.totalPages.set(response.totalPages);

        this.isLoading.set(false);
      },

      error: (error) => {

        console.error('Error loading courses:', error);

        this.errorMessage.set('Unable to load courses.');

        this.isLoading.set(false);
      }
    });
  }

  // Open Add Course Modal
  openAddModal(): void {

    this.form = this.blankForm();

    this.formError.set('');

    this.showFormModal.set(true);
  }

  // Close Add Course Modal
  closeFormModal(): void {

    this.showFormModal.set(false);

    this.formError.set('');
  }

  // Save Course
  saveCourse(): void {

    if (
      !this.form.courseName.trim() ||
      !this.form.duration.trim() ||
      this.form.fees <= 0 ||
      this.form.capacity <= 0
    ) {

      this.formError.set(
        'Please enter all course details correctly.'
      );

      return;
    }

    this.isSubmitting.set(true);
    this.formError.set('');

    this.courseService.createCourse(this.form).subscribe({

      next: () => {

        this.isSubmitting.set(false);

        this.closeFormModal();

        this.loadCourses();
      },

      error: (error) => {

        console.error('Error creating course:', error);

        this.isSubmitting.set(false);

        this.formError.set(
          error.error?.message || 'Unable to create course.'
        );
      }
    });
  }

  // Next Page
  nextPage(): void {

    if (this.pageNumber() < this.totalPages()) {

      this.pageNumber.update(page => page + 1);

      this.loadCourses();
    }
  }

  // Previous Page
  previousPage(): void {

    if (this.pageNumber() > 1) {

      this.pageNumber.update(page => page - 1);

      this.loadCourses();
    }
  }
}
