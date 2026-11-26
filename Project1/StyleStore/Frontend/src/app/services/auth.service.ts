import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthResponse } from '../models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = `${environment.apiUrl}/auth`;
  currentUser = signal<{ fullName: string; email: string } | null>(this.readUser());

  constructor(private http: HttpClient) {}

  register(fullName: string, email: string, password: string) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/register`, { fullName, email, password })
      .pipe(tap(response => this.saveSession(response)));
  }

  login(email: string, password: string) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, { email, password })
      .pipe(tap(response => this.saveSession(response)));
  }

  getToken(): string | null { return localStorage.getItem('styleStoreToken'); }
  logout(): void {
    localStorage.removeItem('styleStoreToken');
    localStorage.removeItem('styleStoreUser');
    this.currentUser.set(null);
  }
  private saveSession(response: AuthResponse): void {
    localStorage.setItem('styleStoreToken', response.token);
    localStorage.setItem('styleStoreUser', JSON.stringify({ fullName: response.fullName, email: response.email }));
    this.currentUser.set({ fullName: response.fullName, email: response.email });
  }
  private readUser() {
    const value = localStorage.getItem('styleStoreUser');
    return value ? JSON.parse(value) as { fullName: string; email: string } : null;
  }
}
