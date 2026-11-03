import { Component } from '@angular/core';
import {
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';

import { AuthService } from './core/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <header class="topbar">
      <a class="brand" routerLink="/">
        <span class="brand-mark">C</span> CineBook
      </a>

      <nav>
        <a routerLink="/" routerLinkActive="active"
           [routerLinkActiveOptions]="{exact:true}">
          Home
        </a>

        <a routerLink="/shows" routerLinkActive="active">
          Shows
        </a>

        @if (auth.hasRole('Admin')) {
          <a routerLink="/admin" routerLinkActive="active">
            Admin
          </a>
        }
      </nav>

      <div class="nav-actions">
        @if (auth.currentUser(); as user) {
          <span class="welcome">Hi, {{ user.userName }}</span>

          <button class="btn btn-outline btn-small"
                  (click)="auth.logout()">
            Logout
          </button>
        } @else {
          <a class="login-link" routerLink="/login">Login</a>

          <a class="btn btn-primary btn-small" routerLink="/register">
            Sign up
          </a>
        }
      </div>
    </header>

    <main>
      <router-outlet />
    </main>

    <footer>
      © 2026 CineBook · Your movie night starts here.
    </footer>
  `
})
export class AppComponent {
  constructor(public auth: AuthService) {}
}