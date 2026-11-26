import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home.component';
import { ProductsComponent } from './pages/products.component';
import { ProductDetailsComponent } from './pages/product-details.component';
import { CartComponent } from './pages/cart.component';
import { LoginComponent } from './pages/login.component';
import { RegisterComponent } from './pages/register.component';
import { OrdersComponent } from './pages/orders.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'products/:id', component: ProductDetailsComponent },
  { path: 'cart', component: CartComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'orders', component: OrdersComponent },
  { path: '**', redirectTo: '' }
];
