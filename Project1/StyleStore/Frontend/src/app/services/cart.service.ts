import { Injectable, computed, signal } from '@angular/core';
import { CartItem, Product } from '../models';

@Injectable({ providedIn: 'root' })
export class CartService {
  private readonly itemsState = signal<CartItem[]>(this.readCart());
  items = this.itemsState.asReadonly();
  totalItems = computed(() => this.itemsState().reduce((sum, item) => sum + item.quantity, 0));
  totalPrice = computed(() => this.itemsState().reduce((sum, item) => sum + item.product.price * item.quantity, 0));

  add(product: Product, size: string): void {
    const current = [...this.itemsState()];
    const existing = current.find(i => i.product.id === product.id && i.size === size);
    if (existing) existing.quantity++;
    else current.push({ product, size, quantity: 1 });
    this.update(current);
  }
  changeQuantity(productId: number, size: string, quantity: number): void {
    if (quantity < 1) return this.remove(productId, size);
    this.update(this.itemsState().map(i =>
      i.product.id === productId && i.size === size ? { ...i, quantity } : i));
  }
  remove(productId: number, size: string): void {
    this.update(this.itemsState().filter(i => !(i.product.id === productId && i.size === size)));
  }
  clear(): void { this.update([]); }
  private update(items: CartItem[]): void {
    this.itemsState.set(items);
    localStorage.setItem('styleStoreCart', JSON.stringify(items));
  }
  private readCart(): CartItem[] {
    const value = localStorage.getItem('styleStoreCart');
    return value ? JSON.parse(value) as CartItem[] : [];
  }
}
