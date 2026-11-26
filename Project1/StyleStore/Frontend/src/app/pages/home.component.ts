import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProductService } from '../services/product.service';
import { Product } from '../models';

@Component({
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="hero">
      <div class="hero-copy">
        <p class="eyebrow">NEW SEASON · NEW ENERGY</p>
        <h1>Wear what<br><em>moves you.</em></h1>
        <p>Thoughtful everyday pieces. Easy fits, fresh looks, and comfort that goes everywhere.</p>
        <a class="button dark" routerLink="/products">Explore collection</a>
      </div>
      <div class="hero-image"><img src="https://images.unsplash.com/photo-1483985988355-763728e1935b?w=1200" alt="Friends exploring fashion" /></div>
    </section>
    <section class="section">
      <div class="section-heading"><div><p class="eyebrow">CURATED FOR YOU</p><h2>Featured pieces</h2></div><a routerLink="/products">View all →</a></div>
      <div class="product-grid">
        @for (p of products; track p.id) {
          <a class="product-card" [routerLink]="['/products', p.id]">
            <img [src]="p.imageUrl" [alt]="p.name" />
            <div class="product-info"><span>{{ p.category }}</span><h3>{{ p.name }}</h3><b>₹{{ p.price }}</b></div>
          </a>
        }
      </div>
    </section>
  `
})
export class HomeComponent implements OnInit {
  private service = inject(ProductService);
  products: Product[] = [];
  ngOnInit() { this.service.getProducts().subscribe(items => this.products = items.filter(p => p.isFeatured).slice(0, 3)); }
}
