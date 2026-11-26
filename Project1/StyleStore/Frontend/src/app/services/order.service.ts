import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Order } from '../models';
import { CartItem } from '../models';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly url = `${environment.apiUrl}/orders`;
  constructor(private http: HttpClient) {}
  placeOrder(items: CartItem[]) {
    const body = { items: items.map(i => ({ productId: i.product.id, size: i.size, quantity: i.quantity })) };
    return this.http.post<Order>(this.url, body);
  }
  getOrders() { return this.http.get<Order[]>(this.url); }
}
