import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Movie, Show, Seat, CreateBookingRequest } from './models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly base = environment.apiBaseUrl;
  constructor(private http: HttpClient) {}

  getMovies() {
    return this.http.get<Movie[]>(`${this.base}/movies`);
  }

  getShows(movieId?: number) {
    let params = new HttpParams();
    if (movieId) params = params.set('movieId', movieId);
    return this.http.get<Show[]>(`${this.base}/shows`, { params });
  }

  getAvailableSeats(showId: number) {
    return this.http.get<Seat[]>(`${this.base}/shows/${showId}/seats/available`);
  }

  createBooking(data: CreateBookingRequest) {
    // Assumes the backend exposes POST /api/bookings.
    return this.http.post(`${this.base}/bookings`, data);
  }
}
