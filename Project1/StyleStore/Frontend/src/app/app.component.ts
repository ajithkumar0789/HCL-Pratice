import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { CartService } from './services/cart.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  template: `
    <header class="navbar">
      <a class="brand" routerLink="/">STYLE<span>STORE</span></a>
      <nav>
        <a routerLink="/products" routerLinkActive="active">Shop</a>
        <a routerLink="/cart" routerLinkActive="active">Bag ({{ cart.totalItems() }})</a>
        @if (auth.currentUser(); as user) {
          <a routerLink="/orders">My Orders</a>
          <span class="welcome">Hi, {{ user.fullName }}</span>
          <button class="link-button" (click)="auth.logout()">Logout</button>
        } @else {
          <a routerLink="/login">Login</a>
          <a class="nav-cta" routerLink="/register">Join us</a>
        }
      </nav>
    </header>
    <main><router-outlet /></main>
    <footer>STYLESTORE · Everyday style, made simple.</footer>
  `
})
export class AppComponent {
  constructor(public auth: AuthService, public cart: CartService) {}
}
