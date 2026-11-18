import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Department, CreateDepartment } from '../models/department';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  private apiUrl = 'http://localhost:5156/api/Department';

  constructor(private http: HttpClient) {}

  // Add Department
  addDepartment(data: CreateDepartment): Observable<Department> {
    return this.http.post<Department>(this.apiUrl, data);
  }

  // Get All Departments
  getAllDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(this.apiUrl);
  }

  // Get Department By ID
  getDepartmentById(id: number): Observable<Department> {
    return this.http.get<Department>(`${this.apiUrl}/${id}`);
  }

  // Update Department
  updateDepartment(id: number, data: CreateDepartment): Observable<Department> {
    return this.http.put<Department>(`${this.apiUrl}/${id}`, data);
  }

  // Delete Department
  deleteDepartment(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}