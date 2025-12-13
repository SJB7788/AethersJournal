# AethersJournal

AethersJournal is a web-based journaling application built with **.NET** and **Blazor** that integrates **AI-assisted reflection** features. 
The application is designed to help users write journal entries and receive structured, supportive feedback intended to encourage reflection and emotional awareness.

This project focuses on maintainable architecture, clear separation of concerns, and privacy-aware design. Features such as analytics and encryption are planned for future iterations.

## Features

- **Journal entries** with free-form text input
- **AI-assisted reflections** (summaries + reflective questions)
- **Journal Analytics** 
- **Private by default** (no public sharing features)
- **Extensible architecture** for future growth

## Design Goals

- Keep the codebase **modular and maintainable**
- Maintain a clear separation between **UI**, **business logic**, and **infrastructure**
- Use AI to assist reflection, not to provide advice or diagnoses 

## Tech Stack

### Frontend
- Blazor
- Razor Components
- CSS styling 

### Backend
- ASP.NET Core
- PostgreSQL
- Entity Framework Core

### Infrastructure
- Environment-based configuration
- Docker support
- Cloud hosting (Azure)

### AI Integration
- Google Gemini API

### Prerequisites
- .NET SDK (LTS)
- PostgreSQL (local or hosted)
- Docker (optional)

### Setup
```sh
git clone https://github.com/SJB7788/AethersJournal.git
cd AethersJournal
dotnet restore
dotnet build
```

### Configuration

Set environment variables or create a .env file:
```.env
AI_API_KEY=your_api_key
DB_CONNECTION_STRING=your_connection_string
```

### Runing the Application
```sh
dotnet run --project src/AethersJournal.csproj
```

## Project Structure

```text
AethersJournal/
├── .github/workflows/      # CI/CD workflows
├── .vscode/                # Editor configuration
├── src/                    # Application source code
│   ├── AI/                 # AI-related logic and integrations
│   ├── Components/         # Reusable Blazor components
│   ├── Controllers/        # ASP.NET Core controllers
│   ├── db/                 # Database configuration and utilities
│   ├── Migrations/         # Entity Framework Core migrations
│   ├── Model/              # Domain and data models
│   ├── Pages/              # Blazor pages
│   ├── Services/           # Application and business services
│   ├── wwwroot/            # Static web assets
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── AethersJournal.csproj
├── docker-compose.yaml     # Local container orchestration
├── AethersJournal.sln      # Solution file
└── README.md
```

## Current Status

This project is under active development.

Core journaling functionality, initial AI integration and user authentication are implemented. 
Calendar and analytics features are in progress.

## Limitations
- This application is not a medical or diagnostic tool
- AI-generated content may be incomplete or inaccurate
- No emergency or crisis handling features are included
