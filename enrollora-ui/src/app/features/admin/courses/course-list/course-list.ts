
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  CourseFilter,
  CourseResponse,
  SaveCourseRequest
} from '../course.models';

import { CourseService } from '../course.service';

type ModalMode = 'add' | 'edit';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './course-list.html',
  styleUrl: './course-list.css',
})
export class CourseListComponent implements OnInit {

  private readonly courseService = inject(CourseService);

  // Course list
  courses = signal<CourseResponse[]>([]);
  isLoading = signal(false);
  errorMessage = signal('');

  pageNumber = signal(1);
  pageSize = signal(10);
  totalRecords = signal(0);
  totalPages = signal(0);

  // Filters
  searchTerm = '';
  statusFilter = '';
  availabilityFilter = '';

  // Add / Edit modal
  showFormModal = signal(false);
  modalMode = signal<ModalMode>('add');
  editingCourseId = signal<number | null>(null);

  isSubmitting = signal(false);
  formError = signal('');

  form: SaveCourseRequest = this.blankForm();

  // Deactivate modal
  showDeactivateModal = signal(false);
  deactivatingCourse = signal<CourseResponse | null>(null);

  isDeactivating = signal(false);
  deactivateError = signal('');

  // Enable course
  enablingCourseId = signal<number | null>(null);

  // Toast
  toastMessage = signal('');
  toastType = signal<'success' | 'error'>('success');

  ngOnInit(): void {
    this.loadCourses();
  }

  // TEST ADD COURSE BUTTON
  testClick(): void {
    console.log('Add Course button clicked!');
    alert('Button is working!');
    this.openAddModal();
  }

  // Load courses
  loadCourses(): void {

    this.isLoading.set(true);
    this.errorMessage.set('');

    const filters: CourseFilter = {
      search: this.searchTerm || undefined,

      isActive:
        this.statusFilter !== ''
          ? this.statusFilter === 'true'
          : undefined,

      availability: this.availabilityFilter || undefined,
    };

    this.courseService
      .getCourses(this.pageNumber(), this.pageSize(), filters)
      .subscribe({

        next: (response) => {
          this.courses.set(response.items);
          this.totalRecords.set(response.totalRecords);
          this.totalPages.set(response.totalPages);
          this.isLoading.set(false);
        },

        error: (err: HttpErrorResponse) => {
          const body = err.error;

          this.errorMessage.set(
            body?.detail ||
            body?.message ||
            body?.title ||
            'Unable to load courses.'
          );

          this.isLoading.set(false);
        }
      });
  }

  // Apply filters
  applyFilter(): void {
    this.pageNumber.set(1);
    this.loadCourses();
  }

  // Pagination
  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(p => p + 1);
      this.loadCourses();
    }
  }

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(p => p - 1);
      this.loadCourses();
    }
  }

  // Add course modal
  openAddModal(): void {
    console.log('Opening Add Course Modal');

    this.modalMode.set('add');
    this.editingCourseId.set(null);
    this.form = this.blankForm();
    this.formError.set('');
    this.showFormModal.set(true);
  }

  // Edit course modal
  openEditModal(course: CourseResponse): void {

    this.modalMode.set('edit');
    this.editingCourseId.set(course.courseId);

    this.form = {
      courseName: course.courseName,
      duration: course.duration,
      fees: course.fees,
      capacity: course.capacity,
      isActive: course.isActive,
    };

    this.formError.set('');
    this.showFormModal.set(true);
  }

  closeFormModal(): void {
    this.showFormModal.set(false);
  }

  // Save course
  submitForm(): void {

    if (!this.form.courseName.trim()) {
      this.formError.set('Course name is required.');
      return;
    }

    if (!this.form.duration.trim()) {
      this.formError.set('Duration is required.');
      return;
    }

    if (this.form.fees < 0) {
      this.formError.set('Fees cannot be negative.');
      return;
    }

    if (this.form.capacity < 1) {
      this.formError.set('Capacity must be at least 1.');
      return;
    }

    this.isSubmitting.set(true);
    this.formError.set('');

    const isAdd = this.modalMode() === 'add';
    const editId = this.editingCourseId();

    const obs$ = isAdd
      ? this.courseService.createCourse(this.form)
      : this.courseService.updateCourse(editId!, this.form);

    obs$.subscribe({

      next: (saved) => {

        this.isSubmitting.set(false);
        this.closeFormModal();

        if (isAdd) {
          this.showToast('Course added successfully!', 'success');
          this.pageNumber.set(1);
          this.loadCourses();
        } else {
          this.showToast('Course updated successfully!', 'success');

          this.courses.update(list =>
            list.map(c =>
              c.courseId === saved.courseId ? saved : c
            )
          );
        }
      },

      error: (err: HttpErrorResponse) => {
        const body = err.error;

        this.formError.set(
          body?.detail ||
          body?.message ||
          body?.title ||
          'Failed to save course.'
        );

        this.isSubmitting.set(false);
      }
    });
  }

  // Deactivate course
  openDeactivateModal(course: CourseResponse): void {
    this.deactivatingCourse.set(course);
    this.deactivateError.set('');
    this.showDeactivateModal.set(true);
  }

  closeDeactivateModal(): void {
    this.showDeactivateModal.set(false);
    this.deactivatingCourse.set(null);
  }

  confirmDeactivate(): void {

    const course = this.deactivatingCourse();

    if (!course) return;

    this.isDeactivating.set(true);
    this.deactivateError.set('');

    this.courseService
      .deactivateCourse(course.courseId)
      .subscribe({

        next: () => {
          this.isDeactivating.set(false);
          this.closeDeactivateModal();

          this.showToast(
            'Course deactivated. Historical enrollments preserved.',
            'success'
          );

          this.courses.update(list =>
            list.map(c =>
              c.courseId === course.courseId
                ? { ...c, isActive: false }
                : c
            )
          );
        },

        error: (err: HttpErrorResponse) => {
          const body = err.error;

          this.deactivateError.set(
            body?.detail ||
            body?.message ||
            body?.title ||
            'Failed to deactivate course.'
          );

          this.isDeactivating.set(false);
        }
      });
  }

  // Enable course
  enableCourse(course: CourseResponse): void {

    if (course.isActive) return;
    if (this.enablingCourseId() !== null) return;

    this.enablingCourseId.set(course.courseId);

    const request: SaveCourseRequest = {
      courseName: course.courseName,
      duration: course.duration,
      fees: course.fees,
      capacity: course.capacity,
      isActive: true,
    };

    this.courseService
      .updateCourse(course.courseId, request)
      .subscribe({

        next: (saved) => {
          this.enablingCourseId.set(null);

          this.courses.update(list =>
            list.map(c =>
              c.courseId === saved.courseId ? saved : c
            )
          );

          this.showToast('Course enabled successfully!', 'success');
        },

        error: (err: HttpErrorResponse) => {
          const body = err.error;

          this.enablingCourseId.set(null);

          this.showToast(
            body?.detail ||
            body?.message ||
            body?.title ||
            'Failed to enable course.',
            'error'
          );
        }
      });
  }

  // Helpers
  seatsClass(course: CourseResponse): string {
    if (course.availableSeats === 0) return 'no-seats';
    if (course.availableSeats <= 5) return 'low-seats';
    return '';
  }

  enrollmentPct(course: CourseResponse): number {
    return course.capacity > 0
      ? Math.min(100, (course.enrolledCount / course.capacity) * 100)
      : 0;
  }

  private blankForm(): SaveCourseRequest {
    return {
      courseName: '',
      duration: '',
      fees: 0,
      capacity: 1,
      isActive: true
    };
  }

  private showToast(
    msg: string,
    type: 'success' | 'error'
  ): void {
    this.toastMessage.set(msg);
    this.toastType.set(type);

    setTimeout(() => this.toastMessage.set(''), 3500);
  }
}
