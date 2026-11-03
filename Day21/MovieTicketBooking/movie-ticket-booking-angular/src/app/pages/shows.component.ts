
import { Component, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { ApiService } from '../core/api.service';
import { Show } from '../core/models';

@Component({
  selector: 'app-shows',
  standalone: true,
  imports: [RouterLink, DatePipe],
  template: `
    <section class="page-heading">
      <span class="eyebrow">CINEMA SCHEDULE</span>
      <h1>Choose your show</h1>
      <p class="muted">
        Pick a movie screening that works for you.
      </p>
    </section>

    @if (loading()) {
      <p class="muted">Loading shows…</p>
    }

    @if (error()) {
      <p class="error">{{ error() }}</p>
    }

    <div class="show-list">
      @for (show of shows(); track show.showId) {
        <article class="show-card">

          <div class="show-symbol">▶</div>

          <div class="show-details">
            <h2>
              {{ show.movieTitle || ('Movie #' + show.movieId) }}
            </h2>

            <p>
              {{ show.theatreName || 'Theatre' }}
              ·
              {{ show.screenName || 'Screen' }}
            </p>

            <p class="show-date">
              {{ show.startTime | date:'EEE, dd MMM yyyy · h:mm a' }}
            </p>
          </div>

          <div class="show-book">
            <strong>₹{{ show.ticketPrice }}</strong>

            <a
              class="btn btn-primary btn-small"
              [routerLink]="['/booking', show.showId]"
            >
              Select seats
            </a>
          </div>

        </article>
      } @empty {
        @if (!loading()) {
          <p class="empty-state">
            No shows available right now.
          </p>
        }
      }
    </div>
  `
})
export class ShowsComponent implements OnInit {

  shows = signal<Show[]>([]);

  loading = signal(true);

  error = signal('');

  constructor(
    private api: ApiService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {

    const id =
      Number(this.route.snapshot.queryParamMap.get('movieId'))
      || undefined;

    this.api.getShows(id).subscribe({

      next: data => {
        this.shows.set(data);
        this.loading.set(false);
      },

      error: () => {
        this.error.set('Could not load shows.');
        this.loading.set(false);
      }

    });
  }
}
