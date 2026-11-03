
import { Component, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ApiService } from '../core/api.service';
import { AuthService } from '../core/auth.service';
import { Seat, Show } from '../core/models';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [FormsModule, DatePipe],
  template: `
    <section class="page-heading">
      <span class="eyebrow">YOUR CINEMA EXPERIENCE</span>
      <h1>Select your seats</h1>
      <p class="muted">
        Choose available seats, then confirm your booking.
      </p>
    </section>

    @if (error()) {
      <p class="error">{{ error() }}</p>
    }

    @if (show(); as s) {
      <div class="booking-layout">

        <section class="seat-panel">
          <div class="screen">SCREEN</div>

          <div class="seat-grid">
            @for (seat of seats(); track seat.seatId) {
              <button
                type="button"
                class="seat"
                [class.selected]="selected().includes(seat.seatId)"
                [class.unavailable]="seat.status && seat.status.toLowerCase() !== 'available'"
                [disabled]="seat.status && seat.status.toLowerCase() !== 'available'"
                (click)="toggleSeat(seat.seatId)"
              >
                {{ seat.rowName || seat.row || '' }}{{ seat.seatNumber ?? seat.number ?? seat.seatId }}
              </button>
            }
          </div>

          <div class="seat-legend">
            <span>□ Available</span>
            <span class="legend-selected">■ Selected</span>
            <span class="legend-taken">■ Unavailable</span>
          </div>
        </section>

        <aside class="booking-summary">
          <h2>Booking summary</h2>

          <h3>{{ s.movieTitle || 'Movie' }}</h3>

          <p>{{ s.theatreName }} · {{ s.screenName }}</p>

          <p>{{ s.startTime | date:'EEE, dd MMM · h:mm a' }}</p>

          <div class="summary-line">
            <span>Selected seats</span>
            <strong>{{ selected().length }}</strong>
          </div>

          <div class="summary-line">
            <span>Price per seat</span>
            <strong>₹{{ s.ticketPrice }}</strong>
          </div>

          <div class="summary-line total">
            <span>Total</span>
            <strong>₹{{ s.ticketPrice * selected().length }}</strong>
          </div>

          <form (ngSubmit)="book()">
            <label>Customer name</label>
            <input
              name="customerName"
              [(ngModel)]="customerName"
              required
            >

            <label>Email</label>
            <input
              name="customerEmail"
              [(ngModel)]="customerEmail"
              required
              type="email"
            >

            <label>Phone (optional)</label>
            <input
              name="customerPhone"
              [(ngModel)]="customerPhone"
            >

            <button
              class="btn btn-primary full"
              type="submit"
              [disabled]="selected().length === 0 || submitting()"
            >
              {{ submitting() ? 'Booking…' : 'Confirm booking' }}
            </button>
          </form>

          @if (message()) {
            <p class="success">{{ message() }}</p>
          }
        </aside>

      </div>
    }
  `
})
export class BookingComponent implements OnInit {

  show = signal<Show | null>(null);

  seats = signal<Seat[]>([]);

  selected = signal<number[]>([]);

  error = signal('');

  message = signal('');

  submitting = signal(false);

  customerName = '';

  customerEmail = '';

  customerPhone = '';

  private showId = 0;

  constructor(
    private route: ActivatedRoute,
    private api: ApiService,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.showId = Number(
      this.route.snapshot.paramMap.get('showId')
    );

    const user = this.auth.currentUser();

    this.customerName = user?.userName || '';

    this.customerEmail = user?.email || '';

    // Load show details
    this.api.getShows().subscribe({
      next: list => {
        this.show.set(
          list.find(x => x.showId === this.showId) || null
        );

        if (!this.show()) {
          this.error.set('Show not found.');
        }
      },
      error: () => {
        this.error.set('Could not load show details.');
      }
    });

    // Load available seats
    this.api.getAvailableSeats(this.showId).subscribe({
      next: data => {
        this.seats.set(data);
      },
      error: () => {
        this.error.set(
          'Could not load available seats. Check the available-seats API response.'
        );
      }
    });
  }

  toggleSeat(id: number): void {

    this.selected.update(ids =>
      ids.includes(id)
        ? ids.filter(x => x !== id)
        : [...ids, id]
    );
  }

  book(): void {

    if (!this.show() || !this.selected().length) {
      return;
    }

    this.submitting.set(true);

    this.error.set('');

    this.message.set('');

    this.api.createBooking({
      showId: this.showId,
      customerName: this.customerName,
      customerEmail: this.customerEmail,
      customerPhone: this.customerPhone || undefined,
      seatIds: this.selected()
    }).subscribe({

      next: () => {
        this.message.set(
          'Booking request submitted successfully.'
        );

        this.submitting.set(false);
      },

      error: err => {
        this.error.set(
          err.error?.message ||
          'Booking failed. Check the booking API route and request DTO.'
        );

        this.submitting.set(false);
      }

    });
  }
}
