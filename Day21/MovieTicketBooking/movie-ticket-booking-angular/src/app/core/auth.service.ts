import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest } from './models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenKey = 'movie_booking_token';
  private readonly userKey = 'movie_booking_user';
  currentUser = signal<AuthResponse | null>(this.readUser());

  constructor(private http: HttpClient) {}

  login(data: LoginRequest) {
    return this.http.post<AuthResponse>(`${environment.apiBaseUrl}/auth/login`, data)
      .pipe(tap(result => this.saveSession(result)));
  }

  register(data: RegisterRequest) {
    return this.http.post(`${environment.apiBaseUrl}/auth/register`, data);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.currentUser.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  hasRole(role: string): boolean {
    return this.currentUser()?.roles?.includes(role) ?? false;
  }

  private saveSession(user: AuthResponse): void {
    localStorage.setItem(this.tokenKey, user.token);
    localStorage.setItem(this.userKey, JSON.stringify(user));
    this.currentUser.set(user);
  }

  private readUser(): AuthResponse | null {
    const value = localStorage.getItem(this.userKey);
    if (!value) return null;
    try { return JSON.parse(value) as AuthResponse; }
    catch { return null; }
  }
}
