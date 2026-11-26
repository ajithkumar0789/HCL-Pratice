import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <section class="auth-card"><p class="eyebrow">WELCOME BACK</p><h1>Sign in.</h1>
      <form (ngSubmit)="submit()"><label>Email</label><input type="email" name="email" [(ngModel)]="email" required />
        <label>Password</label><input type="password" name="password" [(ngModel)]="password" required />
        @if (error) { <p class="error">{{ error }}</p> }
        <button class="button dark full" type="submit">Login</button></form>
      <p class="auth-switch">New to StyleStore? <a routerLink="/register">Create an account</a></p>
    </section>
  `
})
export class LoginComponent {
  private auth = inject(AuthService); private router = inject(Router);
  email = ''; password = ''; error = '';
  submit() { this.auth.login(this.email, this.password).subscribe({ next: () => this.router.navigateByUrl('/orders'), error: e => this.error = e.error?.message || 'Login failed. Check your details.' }); }
}
