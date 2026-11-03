import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../core/auth.service';

@Component({
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <section class="auth-wrap">
      <div class="auth-card">
        <span class="eyebrow">JOIN CINEBOOK</span><h1>Create account</h1>
        <p class="muted">Register as a customer to book your seats.</p>
        <form (ngSubmit)="submit()">
          <label>Full name</label><input name="fullName" [(ngModel)]="fullName" required>
          <label>Username</label><input name="userName" [(ngModel)]="userName" required>
          <label>Email</label><input name="email" [(ngModel)]="email" required type="email">
          <label>Password</label><input name="password" [(ngModel)]="password" required type="password" minlength="6">
          @if (error()) { <p class="error">{{ error() }}</p> }
          @if (success()) { <p class="success">{{ success() }}</p> }
          <button class="btn btn-primary full" [disabled]="loading()">{{ loading() ? 'Creating…' : 'Create account' }}</button>
        </form>
        <p class="auth-switch">Already registered? <a routerLink="/login">Sign in</a></p>
      </div>
    </section>
  `
})
export class RegisterComponent {
  fullName = ''; userName = ''; email = ''; password = '';
  loading = signal(false); error = signal(''); success = signal('');
  constructor(private auth: AuthService, private router: Router) {}
  submit(): void {
    this.loading.set(true); this.error.set('');
    this.auth.register({ fullName: this.fullName, userName: this.userName, email: this.email, password: this.password }).subscribe({
      next: () => { this.success.set('Registration successful. Redirecting to login…'); this.loading.set(false); setTimeout(() => this.router.navigate(['/login']), 900); },
      error: err => { this.error.set(err.error?.message || err.error?.errors?.join(', ') || 'Registration failed.'); this.loading.set(false); }
    });
  }
}
