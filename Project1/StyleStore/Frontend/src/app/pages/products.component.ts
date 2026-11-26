import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProductService } from '../services/product.service';
import { Product } from '../models';

@Component({
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="page-heading"><p class="eyebrow">THE COLLECTION</p><h1>Find your fit.</h1><p>Pieces for every day, every plan.</p></section>
    <div class="filters">
      @for (c of categories; track c) { <button [class.selected]="category === c" (click)="filter(c)">{{ c }}</button> }
    </div>
    <section class="product-grid listing">
      @for (p of products; track p.id) {
        <a class="product-card" [routerLink]="['/products', p.id]">
          <img [src]="p.imageUrl" [alt]="p.name" />
          <div class="product-info"><span>{{ p.category }}</span><h3>{{ p.name }}</h3><b>₹{{ p.price }}</b></div>
        </a>
      } @empty { <p>No products found in this category.</p> }
    </section>
  `
})
export class ProductsComponent implements OnInit {
  private service = inject(ProductService);
  categories = ['All', 'Men', 'Women'];
  category = 'All';
  products: Product[] = [];
  ngOnInit() { this.load(); }
  filter(category: string) { this.category = category; this.load(); }
  load() { this.service.getProducts(this.category).subscribe(items => this.products = items); }
}
