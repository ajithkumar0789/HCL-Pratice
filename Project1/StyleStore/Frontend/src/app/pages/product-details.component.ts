import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../services/product.service';
import { CartService } from '../services/cart.service';
import { Product } from '../models';

@Component({
  standalone: true,
  template: `
    @if (product) {
      <section class="details">
        <img class="details-image" [src]="product.imageUrl" [alt]="product.name" />
        <div class="details-copy">
          <p class="eyebrow">{{ product.category }} · STYLESTORE EDIT</p>
          <h1>{{ product.name }}</h1><h2>₹{{ product.price }}</h2>
          <p>{{ product.description }}</p>
          <label for="size">Choose your size</label>
          <select id="size" #size><option value="">Select size</option>@for (s of sizes; track s) { <option [value]="s">{{ s }}</option> }</select>
          <button class="button dark full" (click)="add(size.value)">Add to bag</button>
          <p class="hint">Easy everyday style. Choose a size before adding to your bag.</p>
        </div>
      </section>
    }
  `
})
export class ProductDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private products = inject(ProductService);
  private cart = inject(CartService);
  product?: Product;
  sizes: string[] = [];
  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.products.getProduct(id).subscribe(p => { this.product = p; this.sizes = p.availableSizes.split(','); });
  }
  add(size: string) {
    if (!this.product || !size) { alert('Please choose a size first.'); return; }
    this.cart.add(this.product, size);
    alert('Added to your bag.');
  }
}
