# Backend Project Documentation

## Overview
This backend is a modular, testable, and scalable ASP.NET Core Web API designed for product management and stock operations. It follows modern DDD and Clean Architecture principles, with a clear separation of concerns between Application, Domain, Infrastructure, and Presentation layers.

---

## Architecture
- **Domain Layer**: Contains core business entities and domain logic (e.g., `Product`).
- **Application Layer**: Contains use case logic, command/query handlers, and DTOs for operations like creating, updating, deleting, and managing product stock.
- **Infrastructure Layer**: Implements repositories, database access (EF Core), and external dependencies.
- **Presentation Layer**: Exposes HTTP endpoints, configures middleware, and handles API versioning and validation.
- **SharedKernel**: Provides cross-cutting concerns such as `Result<T>`, base classes, and interfaces.

### Key Patterns & Practices
- **CQRS**: Commands and queries are handled by dedicated handler classes.
- **Dependency Injection**: All services, handlers, and repositories are registered and injected via DI.
- **FluentValidation**: Used for validating commands and queries.
- **Unit Testing**: Uses xUnit and NSubstitute for isolated and integration tests.
- **API Versioning**: Supports versioned endpoints via URL segments.
- **Soft Delete**: Products are soft-deleted (flagged as deleted, not removed from DB).

---

## Main Libraries & Tools
- **ASP.NET Core**: Web API framework.
- **Entity Framework Core**: ORM for database access.
- **FluentValidation**: Command/query validation.
- **NSubstitute**: Mocking for unit tests.
- **xUnit**: Unit testing framework.
- **Bogus**: Fake data generation for tests.
- **Serilog**: Structured logging.
- **HealthChecks.UI**: Health check endpoints and UI.

---

## ./backend/scripts
Scripts for database migrations and updates are located in `Infrastructure/scripts`:
- `ef-migration.sh`: Creates a new EF Core migration.
- `ef-update.sh`: Applies migrations to update the database.

Usage examples (from project root):
```sh
make migrate MIGRATION_NAME=CreateProductsTable
make db-update
```

---

## Project Structure
```
backend/
├── Application/       # Use cases, commands, handlers, DTOs
├── Domain/            # Business entities and interfaces
├── Infrastructure/    # Repositories, DB context, scripts
│   └── scripts/       # Migration/update shell scripts
├── Presentation/      # Endpoints, middlewares, program entry
├── SharedKernel/      # Common base classes/utilities
├── UnitTests/         # xUnit and NSubstitute-based tests
├── Makefile           # Useful commands for migrations, updates
├── app.sln            # Solution file
└── README.md          # Project documentation
```

---

## How to Run the Project

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- PostgreSQL (or update connection string for your DB)
- (Optional) Docker for containerized DB

### 1. Set Environment Variables
Create a `.env` file or set the following variables:
```
CONNECTION_STRING='Host=localhost;Port=5432;Database=appdb;Username=postgres;Password=postgres;'
```

### 2. Run Database Migrations
```
make migrate MIGRATION_NAME=Initial
make db-update
```

### 3. Build and Run the API
```
dotnet build
cd Presentation
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
The API will be available at `http://localhost:5179` (or the configured port).

Open API Documentation is available at:
http://localhost:5179/swagger/index.html

### 4. Run Tests
```
dotnet test
```

---

## API Endpoints (Main Examples)
- `GET /api/v1/products` - List products
- `POST /api/v1/products` - Create product
- `PUT /api/v1/products/{id}` - Update product
- `DELETE /api/v1/products/{id}` - Soft delete product
- `POST /api/v1/products/{id}/stock` - Add stock
- `DELETE /api/v1/products/{id}/stock` - Reduce stock

---

## Contribution & Extensibility
- All business logic is in handlers and repositories for easy extension.
- Add new features by creating new commands/queries and handlers.
- Use FluentValidation for new DTO validations.
- Add new endpoints in `Presentation/Endpoints/Products/ProductsEndpoint.cs`.

---

## Contact & Support
For questions or contributions, please open an issue or pull request on this repository.
