import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiService } from '../core/api.service';
import { Movie } from '../core/models';

@Component({
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="hero">
      <div class="hero-copy">
        <span class="eyebrow">THE BIG SCREEN EXPERIENCE</span>
        <h1>Stories feel better<br><em>on the big screen.</em></h1>
        <p>Discover movies, choose your seats, and book your next cinema experience.</p>
        <a routerLink="/shows" class="btn btn-primary">Explore shows <span>→</span></a>
      </div>
      <div class="hero-art"><div class="ticket-art">✦<small>YOUR NEXT<br>MOVIE NIGHT</small></div></div>
    </section>
    <section class="section">
      <div class="section-heading"><div><span class="eyebrow">NOW SHOWING</span><h2>Find your next favourite</h2></div><a routerLink="/shows">View all shows →</a></div>
      @if (loading()) { <p class="muted">Loading movies…</p> }
      @if (error()) { <p class="error">{{ error() }}</p> }
      <div class="movie-grid">
        @for (movie of movies(); track movie.movieId) {
          <article class="movie-card">
            <div class="poster"><span class="poster-icon">▶</span><span class="poster-label">{{ movie.language || 'CINEMA' }}</span></div>
            <div class="movie-info"><h3>{{ movie.title }}</h3><p>{{ movie.genre || 'Movie' }} · {{ movie.durationMinutes || '—' }} min</p><a [routerLink]="['/shows']" [queryParams]="{movieId: movie.movieId}" class="text-link">Book tickets →</a></div>
          </article>
        } @empty {
          @if (!loading()) { <p class="muted">No movies found. Check that your API is running.</p> }
        }
      </div>
    </section>
  `
})
export class HomeComponent implements OnInit {
  movies = signal<Movie[]>([]);
  loading = signal(true);
  error = signal('');
  constructor(private api: ApiService) {}
  ngOnInit(): void {
    this.api.getMovies().subscribe({
      next: data => { this.movies.set(data); this.loading.set(false); },
      error: () => { this.error.set('Could not load movies. Check the API and CORS settings.'); this.loading.set(false); }
    });
  }
}
