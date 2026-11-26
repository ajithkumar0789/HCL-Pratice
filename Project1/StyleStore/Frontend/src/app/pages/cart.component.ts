import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartService } from '../services/cart.service';

@Component({
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="page-heading"><p class="eyebrow">YOUR SELECTION</p><h1>Shopping bag</h1></section>
    <section class="cart-layout">
      <div>
        @for (item of cart.items(); track item.product.id + '-' + item.size) {
          <article class="cart-item">
            <img [src]="item.product.imageUrl" [alt]="item.product.name" />
            <div class="cart-item-info"><h3>{{ item.product.name }}</h3><p>Size: {{ item.size }}</p><b>₹{{ item.product.price }}</b>
              <div class="quantity"><button (click)="cart.changeQuantity(item.product.id, item.size, item.quantity - 1)">−</button><span>{{ item.quantity }}</span><button (click)="cart.changeQuantity(item.product.id, item.size, item.quantity + 1)">+</button></div>
              <button class="remove" (click)="cart.remove(item.product.id, item.size)">Remove</button>
            </div>
          </article>
        } @empty { <div class="empty"><h2>Your bag is waiting.</h2><p>Find something that feels like you.</p><a class="button dark" routerLink="/products">Start shopping</a></div> }
      </div>
      @if (cart.items().length) {
        <aside class="summary"><h2>Order summary</h2><p>Items <b>{{ cart.totalItems() }}</b></p><p>Subtotal <b>₹{{ cart.totalPrice() }}</b></p><div class="summary-total">Total <strong>₹{{ cart.totalPrice() }}</strong></div><a class="button dark full" routerLink="/login">Continue to checkout</a><small>Sign in is required to place an order.</small></aside>
      }
    </section>
  `
})
export class CartComponent { cart = inject(CartService); }
