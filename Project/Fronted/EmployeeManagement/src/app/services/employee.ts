
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../models/employee';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private apiUrl = 'http://localhost:5156/api/Employee';

  constructor(private http: HttpClient) {}

  // Add employee
  addEmployee(employee: any): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee);
  }

  // Display all employees
  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  // Search employee by ID
  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }
}
