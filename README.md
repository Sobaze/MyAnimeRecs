# MyAnimeRecs

MyAnimeRecs is a full-stack anime recommendation app that uses your public MyAnimeList profile to:
- import your completed anime list,
- suggest random unseen anime from a broader MAL-based catalog,
- recommend anime to friends based on genres and your completed history.

## Current Scope

This project is intentionally centered on **completed-list-based recommendations**.

- User import currently targets `completed` list entries.
- Friend recommendations are generated from your completed entries only.
- Random recommendations come from a seeded global catalog, excluding anime you have already imported.

## Features

- Import anime from public MAL profiles (official MAL API with `X-MAL-CLIENT-ID`)
- View your imported completed anime list
- Random unseen recommendations (default 3)
- Friend recommendations by selected genres
- Catalog freshness management (backend-owned, 24-hour refresh policy)

## Tech Stack

- Frontend: React, TypeScript, Vite, CSS
- Backend: .NET 8, Minimal APIs, EF Core
- Database: SQLite
- External API: MyAnimeList v2 API

## Architecture

### Backend (`backend/src`)
- `MyAnimeRecs.Api` - HTTP endpoints and app bootstrapping
- `MyAnimeRecs.Application` - interfaces and DTOs
- `MyAnimeRecs.Domain` - core entities and enums
- `MyAnimeRecs.Infrastructure` - EF Core, MAL client, services, background refresh

### Frontend (`frontend/src`)
- `api` - endpoint client and response parsing
- `features` - tab/use-case hooks and feature logic
- `components` - reusable UI components
- `types` - shared API/UI types
- `utils` - normalization and helper utilities

## Prerequisites

- .NET SDK 8.x
- Node.js + npm
- A MAL Client ID

## Configuration

Set your MAL client ID in backend configuration (development settings) so requests send `X-MAL-CLIENT-ID`.

Example location:
- `backend/src/MyAnimeRecs.Api/appsettings.Development.json`

## Getting Started

### 1) Backend

From repo root:
cd backend
```bash
dotnet run --project src/MyAnimeRecs.Api
```
### 2) Frontend
cd frontend
npm install
npm run dev
