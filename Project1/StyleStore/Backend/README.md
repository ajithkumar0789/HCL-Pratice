# StyleStore API

## Requirements
- .NET 10 SDK
- SQL Server / SQL Server Express
- Visual Studio 2026 or VS Code

## Configure SQL Server
Open `StyleStore.Api/appsettings.json` and update `DefaultConnection` to your SQL Server instance.
For example, if your SQL Server instance is `ajith\SQLEXPRESS01`:

`Server=ajith\SQLEXPRESS01;Database=StyleStoreDb;Trusted_Connection=True;TrustServerCertificate=True;`

Change the development JWT key before using this outside a local learning project.

## Run
From `Backend/StyleStore.Api`:

```cmd
dotnet restore
dotnet run
```

Open the Swagger URL printed in the terminal. The app uses `EnsureCreated()` to create a beginner demo database and seed products on first run. For a production project, replace this with EF Core migrations.

## API routes
- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/products`
- `GET /api/products?category=Men`
- `GET /api/products/{id}`
- `POST /api/orders` (Bearer token required)
- `GET /api/orders` (Bearer token required)
