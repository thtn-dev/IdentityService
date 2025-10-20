# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is an ASP.NET Core 9.0 Identity Service using OpenIddict for OAuth2/OpenID Connect implementation. The project provides user authentication, authorization, and identity management with PostgreSQL as the database backend.

## Architecture

The application follows a layered architecture:

- **DataAccess/**: Entity Framework Core with PostgreSQL, including migrations and data context
  - `AppDbContext.cs`: Main DbContext inheriting from IdentityDbContext with OpenIddict integration
  - `DataSets/`: Entity models for Identity and OpenIddict
  - `Migrations/`: EF Core database migrations
- **Extensions/**: Service configuration and dependency injection extensions
  - `ServiceCollections/`: Extension methods for configuring services (EF Core, OpenIddict, Quartz)
- **Controllers/**: API controllers, including OAuth2 endpoints
- **Areas/Identity/Pages/**: Razor Pages for user authentication flows (login, register, manage account)
- **BackgroundServices/**: Hosted services (e.g., `SeedDataWorker`)
- **Business/**: Business logic layer (currently minimal)

## Key Technologies

- ASP.NET Core 9.0 with Identity
- OpenIddict for OAuth2/OpenID Connect
- Entity Framework Core with PostgreSQL (Npgsql)
- Quartz.NET for background job scheduling
- MailKit for email services
- Razor Pages with MVC pattern

## Development Commands

### Building and Running
```bash
# Build the solution
dotnet build IdentityService.sln

# Run the application (development)
dotnet run --project IdentityService

# Run with specific profile
dotnet run --project IdentityService --launch-profile https
```

### Database Operations
```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project IdentityService

# Update database
dotnet ef database update --project IdentityService

# Drop database (be careful!)
dotnet ef database drop --project IdentityService
```

### Development Environment
- **HTTP**: http://localhost:5039
- **HTTPS**: https://localhost:7103
- **Database**: PostgreSQL (connection string in appsettings.Development.json)

## Configuration

- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development-specific settings (not in repo)
- `launchSettings.json`: Development server profiles
- Connection string key: `DefaultConnection`

## Authentication Flow

The application uses ASP.NET Core Identity with OpenIddict:
- Cookie authentication for web UI (`dpn_auth` cookie)
- OAuth2/OpenID Connect for API access
- Custom login path: `/identity/account/login`
- Anti-forgery tokens using `dpn_xsrf` cookie

## Database Schema

Uses Entity Framework Core with:
- ASP.NET Core Identity tables (Users, Roles, etc.)
- OpenIddict tables (Applications, Authorizations, Scopes, Tokens)
- Custom entities in `DataAccess/DataSets/`

## Background Services

- **SeedDataWorker**: Initializes default data on application startup
- **Quartz.NET**: Configured for scheduled background tasks

## CORS Configuration

Currently configured with permissive CORS policy (`*`) - review for production deployment.