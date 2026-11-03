import { Routes } from '@angular/router';

import { authGuard } from './core/auth.guard';
import { adminGuard } from './core/admin.guard';

import { HomeComponent } from './pages/home.component';
import { LoginComponent } from './pages/login.component';
import { RegisterComponent } from './pages/register.component';
import { ShowsComponent } from './pages/shows.component';
import { BookingComponent } from './pages/booking.component';
import { AdminDashboardComponent } from './pages/admin-dashboard.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },

  { path: 'login', component: LoginComponent },

  { path: 'register', component: RegisterComponent },

  { path: 'shows', component: ShowsComponent },

  {
    path: 'booking/:showId',
    component: BookingComponent,
    canActivate: [authGuard]
  },

  {
    path: 'admin',
    component: AdminDashboardComponent,
    canActivate: [adminGuard]
  },

  { path: '**', redirectTo: '' }
];