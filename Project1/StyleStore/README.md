# StyleStore – Online Clothing Shop

Beginner-friendly full-stack starter built with Angular 20, ASP.NET Core Web API (.NET 10), EF Core and SQL Server.

## Features included
- Home page with featured products
- Men / Women product listing and product details
- Size selection and shopping cart (browser local storage)
- Customer registration and login using ASP.NET Core Identity + JWT
- Place orders and view order history
- Responsive fashion-store UI

## Exactly four Angular application services
| Service | Responsibility |
|---|---|
| AuthService | Register, login, logout and token access |
| ProductService | Read products and product details |
| CartService | Add, update and remove cart items |
| OrderService | Place orders and view order history |

The HTTP interceptor is a separate Angular interceptor function, not an application service. Angular framework providers such as HttpClient and Router are not custom application services.

## Project folders
- `Backend/StyleStore.Api` – ASP.NET Core Web API
- `Frontend/src` – Angular application source files

## Setup order
1. Install .NET 10 SDK, Node.js LTS, Angular CLI 20 and SQL Server.
2. Configure the SQL Server connection string in `Backend/StyleStore.Api/appsettings.json`.
3. Run the API:
   ```cmd
   cd Backend\StyleStore.Api
   dotnet restore
   dotnet run
   ```
4. Create the Angular workspace and copy the provided source:
   ```cmd
   cd ..\..\Frontend
   ng new style-store --standalone --routing --style=css
   ```
   Choose no SSR. Copy this folder's `src` contents into the generated `style-store/src` folder.
5. In `Frontend/style-store/src/environments/environment.ts`, set `apiUrl` to the HTTPS URL printed by the API (default template uses `https://localhost:7133/api`).
6. Run Angular:
   ```cmd
   cd style-store
   npm install
   ng serve
   ```

## Important learning notes
- The API demo uses `EnsureCreated()` so the first run creates tables and sample products automatically. This is convenient for a first practice run. Learn EF Core migrations next before changing the schema.
- The cart is stored in localStorage for this beginner version. Orders are saved in SQL Server.
- This is a learning starter, not a production payment system. It does not collect card details or integrate a payment gateway.
- Change the JWT signing key and use user secrets/environment variables before deployment.
- Product images are remote Unsplash URLs and need an internet connection.
