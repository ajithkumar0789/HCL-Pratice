# CineBook Angular Frontend

Angular standalone frontend starter for the MovieTicketBooking ASP.NET Core API.

## Requirements
- Node.js LTS and npm
- Angular CLI 20 (installed locally by npm)

## Run
```bash
npm install
npm start
```
Open http://localhost:4200

## Backend
The API base URL is configured in `src/environments/environment.ts`:
`http://localhost:5223/api`

The frontend expects these endpoints:
- `GET /api/movies`
- `GET /api/shows` (optional `movieId` query parameter)
- `GET /api/shows/{showId}/seats/available`
- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/bookings` (assumed booking route; confirm against your BookingsController)

JWT is stored in browser localStorage for this learning project and attached as a Bearer token by the HTTP interceptor. For production applications, consider a more secure token-storage architecture and HTTPS.

## Enable CORS in ASP.NET Core
In `Program.cs`, before `builder.Build()`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```
After `var app = builder.Build();` and before authentication/authorization middleware:
```csharp
app.UseCors("AngularClient");
```
Keep `UseCors` before `UseAuthentication` and `UseAuthorization`.

## Important backend contract notes
- The booking POST route and response depend on your `BookingsController`.
- The available-seat DTO should expose `seatId`, `rowName` or `row`, `seatNumber` or `number`, and optionally `status`. Adjust `Seat` in `src/app/core/models.ts` if your DTO uses different property names.
- If your API uses HTTPS, update `apiBaseUrl` to its HTTPS address and trust the local development certificate.
