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
        <span class="eyebrow">WELCOME BACK</span><h1>Sign in</h1>
        <p class="muted">Sign in to continue your movie experience.</p>
        <form (ngSubmit)="submit()">
          <label>Username or email</label>
          <input name="userNameOrEmail" [(ngModel)]="username" required placeholder="Enter username or email">
          <label>Password</label>
          <input name="password" [(ngModel)]="password" required type="password" placeholder="Enter password">
          @if (error()) { <p class="error">{{ error() }}</p> }
          <button class="btn btn-primary full" [disabled]="loading()">{{ loading() ? 'Signing in…' : 'Sign in' }}</button>
        </form>
        <p class="auth-switch">New to CineBook? <a routerLink="/register">Create an account</a></p>
      </div>
    </section>
  `
})
export class LoginComponent {
  username = ''; password = '';
  loading = signal(false); error = signal('');
  constructor(private auth: AuthService, private router: Router) {}
  submit(): void {
    this.loading.set(true); this.error.set('');
    this.auth.login({ userNameOrEmail: this.username, password: this.password }).subscribe({
      next: () => this.router.navigate(['/shows']),
      error: err => { this.error.set(err.error?.message || 'Login failed. Check your details.'); this.loading.set(false); }
    });
  }
}
