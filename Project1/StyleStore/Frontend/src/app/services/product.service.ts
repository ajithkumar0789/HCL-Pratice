import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Product } from '../models';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly url = `${environment.apiUrl}/products`;
  constructor(private http: HttpClient) {}
  getProducts(category = 'All') {
    return this.http.get<Product[]>(this.url, { params: category === 'All' ? {} : { category } });
  }
  getProduct(id: number) {
    return this.http.get<Product>(`${this.url}/${id}`);
  }
}
