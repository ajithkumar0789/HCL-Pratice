import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <section class="auth-card"><p class="eyebrow">JOIN THE COMMUNITY</p><h1>Create account.</h1>
      <form (ngSubmit)="submit()"><label>Full name</label><input name="fullName" [(ngModel)]="fullName" required />
        <label>Email</label><input type="email" name="email" [(ngModel)]="email" required />
        <label>Password (minimum 6 characters)</label><input type="password" name="password" [(ngModel)]="password" minlength="6" required />
        @if (error) { <p class="error">{{ error }}</p> }
        <button class="button dark full" type="submit">Create account</button></form>
      <p class="auth-switch">Already a member? <a routerLink="/login">Sign in</a></p>
    </section>
  `
})
export class RegisterComponent {
  private auth = inject(AuthService); private router = inject(Router);
  fullName = ''; email = ''; password = ''; error = '';
  submit() { this.auth.register(this.fullName, this.email, this.password).subscribe({ next: () => this.router.navigateByUrl('/'), error: e => this.error = Array.isArray(e.error) ? e.error.join(', ') : 'Registration failed. Check your details.' }); }
}
