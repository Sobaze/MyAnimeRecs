# MyAnimeRecs

A fullstack web application for anime recommendations based on your MyAnimeList profile. It is based on your completed list and will give you random recommendations you have not seen.
You can also use it to give recommendation to friends, entering the genres they like and then it will go by animes that is in your completed list to get a recommendation.

## Features
- Fetches your anime history via the MyAnimeList API
- Generates recommendations based on what you have already watched and rated
- Random anime generator for titles you have not seen yet
- Recommends anime to friends based on their preferred genres

## Tech Stack
- **Frontend:** React, TypeScript, CSS
- **Backend:** .NET / C#
- **Database:** SQLite
- **API:** MyAnimeList (MAL)

## Getting Started
Frontend:  cd frontend -> npm run dev

Backend: cd backend -> dotnet run --project src/MyAnimeRecs.Api
