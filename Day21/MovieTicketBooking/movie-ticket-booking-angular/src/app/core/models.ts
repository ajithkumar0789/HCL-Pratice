export interface LoginRequest {
  userNameOrEmail: string;
  password: string;
}
export interface RegisterRequest {
  fullName: string;
  userName: string;
  email: string;
  password: string;
}
export interface AuthResponse {
  token: string;
  expiration: string;
  userId: string;
  userName: string;
  email: string;
  roles: string[];
}
export interface Movie {
  movieId: number;
  title: string;
  description?: string;
  durationMinutes?: number;
  releaseDate?: string;
  language?: string;
  genre?: string;
  posterUrl?: string;
}
export interface Show {
  showId: number;
  movieId: number;
  movieTitle?: string;
  screenId?: number;
  screenName?: string;
  theatreId?: number;
  theatreName?: string;
  startTime: string;
  endTime?: string;
  ticketPrice: number;
}
export interface Seat {
  seatId: number;
  rowName?: string;
  row?: string;
  seatNumber?: number;
  number?: number;
  seatType?: string;
  type?: string;
  status?: string;
}
export interface CreateBookingRequest {
  showId: number;
  customerName: string;
  customerEmail: string;
  customerPhone?: string;
  seatIds: number[];
}
