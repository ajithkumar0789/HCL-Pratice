import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="page-heading">
      <span class="eyebrow">ADMINISTRATION</span>
      <h1>Admin Dashboard</h1>
      <p class="muted">
        Manage your CineBook movie booking application.
      </p>
    </section>

    <section class="show-list">
      <article class="show-card">
        <div class="show-details">
          <h2>Movies</h2>
          <p>Add and manage movies.</p>
        </div>
        <a class="btn btn-primary btn-small" routerLink="/admin/movies">
          Manage
        </a>
      </article>

      <article class="show-card">
        <div class="show-details">
          <h2>Theatres</h2>
          <p>Add and manage theatres.</p>
        </div>
        <a class="btn btn-primary btn-small" routerLink="/admin/theatres">
          Manage
        </a>
      </article>

      <article class="show-card">
        <div class="show-details">
          <h2>Screens</h2>
          <p>Manage screens inside theatres.</p>
        </div>
        <a class="btn btn-primary btn-small" routerLink="/admin/screens">
          Manage
        </a>
      </article>

      <article class="show-card">
        <div class="show-details">
          <h2>Seats</h2>
          <p>Create and manage theatre seats.</p>
        </div>
        <a class="btn btn-primary btn-small" routerLink="/admin/seats">
          Manage
        </a>
      </article>

      <article class="show-card">
        <div class="show-details">
          <h2>Shows</h2>
          <p>Schedule movie screenings.</p>
        </div>
        <a class="btn btn-primary btn-small" routerLink="/admin/shows">
          Manage
        </a>
      </article>

      <article class="show-card">
        <div class="show-details">
          <h2>Bookings</h2>
          <p>View customer bookings.</p>
        </div>
        <a class="btn btn-primary btn-small" routerLink="/admin/bookings">
          Manage
        </a>
      </article>
    </section>
  `
})
export class AdminDashboardComponent {}