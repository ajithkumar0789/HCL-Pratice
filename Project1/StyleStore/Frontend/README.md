# StyleStore Angular UI

Create the Angular workspace with Angular CLI 20:

```cmd
npm install -g @angular/cli@20
ng new style-store --standalone --routing --style=css
```

Choose no SSR. Copy the contents of this `src` folder into the generated project's `src` folder, replacing files when prompted. Then run:

```cmd
npm install
ng serve
```

Angular runs at `http://localhost:4200`. The API base URL is configured in `src/environments/environment.ts`.

This starter intentionally contains exactly four application services:
1. `AuthService`
2. `ProductService`
3. `CartService`
4. `OrderService`

Angular's built-in `HttpClient`, Router and Forms modules are framework features, not additional application services.
