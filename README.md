# MyAnimeRecs

A personal full-stack anime recommendation app built around my public MyAnimeList profile.

- Import your completed anime list from MAL
- Get personalized recommendations based on what you've already seen
- Generate random unseen anime picks
- Recommend anime to friends based on genres you pick from your completed history

##Demo

https://github.com/user-attachments/assets/010d2e6a-b185-49cf-8507-9dbac5c89e3b

## Scope

This project is intentionally centered on completed-list-based recommendations.
MAL profiles must be public. OAuth login is not implemented in v1.

## Features

- MAL import via official MAL API (`X-MAL-CLIENT-ID`)
- View your imported completed anime list with sorting (title, user score, MAL score)
- Random unseen recommendations
- Friend recommendations by selected genres
- Catalog freshness managed by the backend (24h refresh policy)

## Tech Stack

- **Frontend:** React, TypeScript, Vite
- **Backend:** .NET 8, Minimal APIs, EF Core
- **Database:** SQLite
- **External API:** MyAnimeList v2

## Architecture

### Backend (`backend/src`)
- `MyAnimeRecs.Api` — endpoint registration and app startup
- `MyAnimeRecs.Application` — service abstractions and DTO contracts
- `MyAnimeRecs.Domain` — core entities and enums
- `MyAnimeRecs.Infrastructure` — EF Core, MAL client, services, background catalog refresh

### Frontend (`frontend/src`)
- `api` — typed API client and endpoint map
- `features` — tab-specific hooks and logic
- `components` — reusable UI components
- `types` — shared API/UI types
- `utils` — normalization and helper functions

## Prerequisites

- .NET SDK 8.x
- Node.js + npm
- A MAL Client ID ([register here](https://myanimelist.net/apiconfig))

## Getting Started

### 1. Set up your MAL Client ID

This project uses ASP.NET user-secrets for local development:
```bash
cd backend/src/MyAnimeRecs.Api
dotnet user-secrets init
dotnet user-secrets set "Mal:ClientId" "YOUR_MAL_CLIENT_ID"
```
### 2. Set up the database
```bash
cd backend
dotnet tool run dotnet-ef database update --project "src/MyAnimeRecs.Infrastructure/MyAnimeRecs.Infrastructure.csproj" --startup-project "src/MyAnimeRecs.Api/MyAnimeRecs.Api.csproj"
```

### 3. Run the backend
```bash
cd backend
dotnet run --project src/MyAnimeRecs.Api
```

### 4. Run the frontend
```bash
cd frontend
npm install
npm run dev
```


> Only needed on first run or after pulling new migrations. If you're adding schema changes yourself, see [EF Core migrations docs](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/).

## API Overview

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/import/mal/{username}` | Import MAL completed list |
| GET | `/api/users/{username}/anime-list` | Get imported anime list |
| GET | `/api/users/{username}/top-genres` | Get top genres from completed list |
| POST | `/api/recommendations/by-mal/{username}` | Personalized recommendations |
| GET | `/api/recommendations/random/{username}` | Random unseen recommendations |
| POST | `/api/recommendations/for-friend/{username}` | Genre-based friend recommendations |

## Known Constraints

- MAL profile must be public
- No OAuth in v1 — import is username-based only
- Recommendations are based on completed list entries only

## What I'd improve next

- Request cancellation for rapid tab switching
- Better empty/error/loading visuals
- Config-driven constants for refresh windows and recommendation caps
- Optional MAL OAuth support
