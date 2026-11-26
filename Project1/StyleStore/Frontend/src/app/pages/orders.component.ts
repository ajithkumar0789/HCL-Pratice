import { Component, OnInit, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrderService } from '../services/order.service';
import { CartService } from '../services/cart.service';
import { AuthService } from '../services/auth.service';
import { Order } from '../models';

@Component({
  standalone: true,
  imports: [RouterLink, DatePipe],
  template: `
    <section class="page-heading"><p class="eyebrow">THE GOOD THINGS YOU PICKED</p><h1>Your orders.</h1></section>
    @if (!auth.currentUser()) {
      <div class="empty"><p>Please log in to view your orders.</p><a routerLink="/login" class="button dark">Login</a></div>
    } @else {
      @if (orders.length) {
        @for (order of orders; track order.id) {
          <article class="order-card"><div class="order-top"><b>Order #{{ order.id }}</b><span>{{ order.status }}</span></div><p>{{ order.orderDate | date:'medium' }}</p>
            @for (item of order.items; track $index) { <p>{{ item.productName }} · {{ item.size }} · Qty {{ item.quantity }} — ₹{{ item.unitPrice * item.quantity }}</p> }
            <h3>Total: ₹{{ order.totalAmount }}</h3>
          </article>
        }
      } @else { <div class="empty"><h2>No orders yet.</h2><a routerLink="/products" class="button dark">Explore products</a></div> }
      @if (cart.items().length) { <button class="button dark place-order" (click)="placeOrder()">Place order · ₹{{ cart.totalPrice() }}</button> }
    }
  `
})
export class OrdersComponent implements OnInit {
  private service = inject(OrderService);
  cart = inject(CartService); auth = inject(AuthService);
  orders: Order[] = [];
  ngOnInit() { this.load(); }
  load() { this.service.getOrders().subscribe(items => this.orders = items); }
  placeOrder() {
    this.service.placeOrder(this.cart.items()).subscribe({
      next: () => { this.cart.clear(); this.load(); alert('Your order has been placed.'); },
      error: () => alert('Could not place your order. Please try again.')
    });
  }
}
