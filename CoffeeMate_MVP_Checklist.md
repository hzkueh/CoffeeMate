# CoffeeMate — MVP Build Checklist

A full-stack portfolio project for Senior / Full Stack .NET Developer roles.
**Stack:** Vue 3 + TypeScript + ASP.NET Core Web API + SignalR

---

## Phase 1 — Planning & Architecture

- [ ] Define solution structure (monorepo or separate repos for frontend/backend)
- [ ] Sketch basic data models: `Coffee`, `Recipe`, `Step`, `Session`, `Collaborator`, `GuestToken`, `User`
- [ ] Define role permissions (see Role Matrix below)
- [ ] Decide on database: **PostgreSQL** (recommended) or SQLite for local dev
- [ ] Choose frontend: **Vue 3 + TypeScript**
- [ ] Set up GitHub repo with a clear README

---

## Phase 2 — Backend Setup (ASP.NET Core)

- [ ] Create ASP.NET Core Web API project
  ```
  dotnet new webapi -n CoffeeMate.API
  ```
- [ ] Install dependencies
  - `Microsoft.EntityFrameworkCore` + provider (Npgsql or SQLite)
  - `Microsoft.AspNetCore.SignalR`
  - `Swashbuckle.AspNetCore` (Swagger)
  - `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (user management)
  - `Microsoft.AspNetCore.Authentication.JwtBearer` (JWT auth)
- [ ] Set up `AppDbContext` with EF Core
- [ ] Configure CORS for local frontend dev (`localhost:3000` or `5173`)
- [ ] Add Swagger UI for API testing
- [ ] Create initial EF Core migration and seed data (a few coffee types)

---

## Phase 3 — Authentication & Role Setup (JWT)

> Registered users get JWT. Guests use a lightweight session token — no JWT needed.

**Role Matrix:**

| Action | Guest | Registered User |
|---|---|---|
| View completed coffee | ✅ | ✅ |
| Contribute to making coffee | ✅ | ✅ |
| Create coffee task | ❌ | ✅ |
| Share / invite collaborators | ❌ | ✅ |

- [ ] Set up `ASP.NET Core Identity` with `AppUser` extending `IdentityUser`
- [ ] Configure JWT in `appsettings.json` (secret key, issuer, expiry)
- [ ] Add JWT middleware in `Program.cs`
  ```csharp
  builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => { ... });
  ```
- [ ] Create auth endpoints:
  - `POST /api/auth/register` — register new user, return JWT
  - `POST /api/auth/login` — login, return JWT
- [ ] Protect registered-user-only endpoints with `[Authorize]`
- [ ] Leave guest endpoints unauthenticated — validate via `GuestToken` instead
- [ ] Pass JWT in SignalR connection (query string or header)
  ```javascript
  new HubConnectionBuilder()
    .withUrl("/coffeeHub", { accessTokenFactory: () => token })
  ```
- [ ] Frontend: store JWT in `localStorage`, attach to Axios requests via interceptor
- [ ] Add `AuthGuard` route guard in Vue Router for protected pages (e.g. create coffee task)

---

## Phase 4 — Core API Endpoints (Feature 1)

> Users can explore coffees, view details, and see recipes.

- [ ] `GET /api/coffees` — list all coffees
- [ ] `GET /api/coffees/{id}` — coffee detail + recipe steps
- [ ] `GET /api/recipes/{id}` — full recipe with ordered steps
- [ ] Seed at least 3–5 coffee types (Espresso, Latte, Cappuccino, etc.)
- [ ] Add unit tests for service layer (xUnit)

---

## Phase 5 — Frontend Setup

- [ ] Scaffold project: `npm create vite@latest coffeemate-web -- --template vue-ts`
- [ ] Install dependencies: `axios`, `vue-router`, `pinia` (state management), Tailwind CSS or plain CSS
- [ ] Set up routing:
  - `/` — Home / Coffee Explorer
  - `/login` — Login / Register
  - `/coffee/:id` — Coffee Detail + Recipe
  - `/simulate/:id` — Simulation Page
  - `/session/:id` — Collaborative Session
- [ ] Set up Axios interceptor to attach JWT to all requests
- [ ] Set up Pinia store for `authStore` (user, token, isLoggedIn)

---

## Phase 6 — Coffee Explorer UI (Feature 1)

- [ ] Build `CoffeeList` component — grid/list of coffees
- [ ] Build `CoffeeDetail` component — name, description, origin, image
- [ ] Build `RecipeSteps` component — ordered step-by-step instructions
- [ ] Connect to backend API via Axios/fetch
- [ ] Add loading states and basic error handling

---

## Phase 7 — Simulation Mode (Feature 2)

> Simple mode: simulate the coffee-making process step by step.

- [ ] Build `SimulationEngine` — state machine with steps (e.g. Grind → Brew → Pour → Done)
- [ ] Build `SimulationView` component with:
  - [ ] Visual progress indicator (step bar or animation)
  - [ ] "Next Step" button to advance
  - [ ] Step description displayed per stage
- [ ] Add `POST /api/sessions` endpoint to create a new simulation session
- [ ] Persist simulation state to DB (optional for MVP, good for collab later)

---

## Phase 8 — Guest Mode

> No login required — guests click a button and get assigned a coffee-themed username.

- [ ] Generate guest token on the backend on session join
  - `POST /api/sessions/{id}/join-guest` → returns `{ guestToken, assignedUsername }`
  - Username format: coffee-themed adjective + role + number (e.g. `Grinder124`, `Brewer032`, `Roaster77`)
  - Store `GuestToken` in DB with `SessionId`, `AssignedUsername`, `CreatedAt`, `IsActive`
- [ ] Save guest token in browser `localStorage` so they can rejoin
- [ ] Build `GuestJoin` component — single "Join & Help Make Coffee" button
- [ ] On rejoin (token exists in localStorage):
  - `POST /api/sessions/{id}/rejoin` with token → validate token → return current session state
  - If session still active → redirect to step picker
  - If session completed → redirect to summary directly
- [ ] Guest session **expires** when coffee is made (set `IsActive = false`)
- [ ] Guest `AssignedUsername` **persists** in the collaborators summary after session ends

---

## Phase 9 — Real-Time Collaborative Coffee Making (Feature 4)

> Multiple users can participate in making the same coffee in real time.

- [ ] Create `CoffeeHub` (SignalR Hub) in the backend
  ```csharp
  public class CoffeeHub : Hub
  {
      public async Task JoinSession(string sessionId, string guestToken) { ... }
      public async Task ClaimStep(string sessionId, string stepId, string guestToken) { ... }
      public async Task CompleteStep(string sessionId, string stepId, string guestToken) { ... }
  }
  ```
- [ ] Build **step claiming logic** (first come first serve)
  - `ClaimStep` locks the step to the first guest who calls it
  - Use optimistic concurrency with EF Core (`RowVersion` / `ConcurrencyToken`) to prevent double-claim
  - Broadcast `StepClaimed` event to all session participants via SignalR
  - If step already claimed → return conflict response, notify the late guest
- [ ] Handle **rejoin step reassignment**
  - On rejoin → fetch list of `AvailableSteps` (unclaimed, incomplete)
  - Show guest a **step picker UI** — they choose which step to help with
  - If no steps available → show waiting state or summary if done
- [ ] Frontend: install `@microsoft/signalr`
- [ ] Build `CollabSession` component:
  - [ ] Join session by ID or share link
  - [ ] Show connected collaborators (avatars or assigned usernames)
  - [ ] **Step Picker UI** — list of available steps, click to claim
  - [ ] Claimed step shows as locked with the claimer's username in real time
  - [ ] Broadcast step completion to all participants
  - [ ] Conflict UI: notify guest if their chosen step was just taken

---

## Phase 10 — Sharing & Collaborators View (Feature 5)

- [ ] `GET /api/sessions/{id}/summary` — returns completed coffee + list of contributors
- [ ] Build `SessionSummary` component:
  - [ ] Display finished coffee name
  - [ ] Show who contributed to each step
  - [ ] Shareable link (copy to clipboard)
- [ ] Store collaborator actions per step in DB

---

## Phase 11 — Polish & Portfolio Readiness

- [ ] Add a clean landing page (hero section, what the app does)
- [ ] Responsive design (mobile-friendly)
- [ ] Add error boundaries / fallback UI
- [ ] Write a proper `README.md`:
  - [ ] Project overview
  - [ ] Tech stack
  - [ ] How to run locally
  - [ ] Architecture diagram (optional but impressive)
- [ ] Add comments/XML docs to key backend services

---

## Phase 12 — Deployment (Optional, $0 Tier)

- [ ] Frontend → deploy to **Vercel** or **Netlify** (free)
- [ ] Backend → deploy to **Railway** or **Render** (free tier)
- [ ] Set environment variables (DB connection string, CORS origin)
- [ ] Update CORS config for production frontend URL
- [ ] Test SignalR over deployed environment (check WebSocket support on host)

---

## Tech Stack Summary

| Layer | Technology |
|---|---|
| Frontend | Vue 3 + TypeScript + Pinia |
| Backend | ASP.NET Core Web API (.NET 8) |
| Auth | ASP.NET Identity + JWT (registered) / GuestToken (guests) |
| Real-Time | SignalR |
| ORM | Entity Framework Core |
| Database | PostgreSQL (prod) / SQLite (local) |
| Hosting | Vercel (FE) + Railway/Render (BE) |

---

## Key Talking Points for Interviews

- **Dual auth strategy** — JWT for registered users, lightweight GuestToken for guests, both handled in the same SignalR hub
- **SignalR + real-time step claiming** — first come first serve with live broadcast to all participants
- **Optimistic concurrency** — EF Core `RowVersion` preventing double-claim race conditions
- **Guest session design** — stateless token-based guest flow with smart rejoin logic
- **State machine pattern** — simulation engine design
- **Clean API design** — RESTful endpoints with `[Authorize]` + open guest endpoints
- **Separation of concerns** — service layer, repository pattern (if used)
- **UX-driven backend logic** — step reassignment on rejoin based on current session state
