# LastLink Backend

## Overview

This is the backend service for the LastLink platform, built with .NET 9.0 and following Clean Architecture principles. It provides APIs for managing advance payment requests for creators with a 5% fee calculation.

## Key Features

- **Advance Request Management**: Create, view, approve, and reject advance payment requests
- **Creator-Specific Requests**: View all advance requests for a specific creator
- **Automatic Fee Calculation**: 5% fee is automatically calculated on each advance request
- **Request Status Tracking**: Track the status of each advance request (Pending, Approved, Rejected)
- **RESTful API**: Clean, consistent API design following REST principles
- **API Versioning**: Support for multiple API versions
- **Health Checks**: Built-in health check endpoints
- **Swagger/OpenAPI**: Interactive API documentation

## Technology Stack

- **.NET 9.0**: Cross-platform, high-performance framework
- **Entity Framework Core**: ORM for data access
- **PostgreSQL**: Relational database
- **MediatR**: In-process messaging for implementing CQRS
- **FluentValidation**: Validation library
- **xUnit**: Unit testing framework
- **Serilog**: Logging framework
- **Docker**: Containerization

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/products/docker-desktop) (for running PostgreSQL in development)
- [Node.js](https://nodejs.org/) (for running the frontend)

### Development Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/riguelbf/lastlink.git
   cd lastlink/backend
   ```

2. **Set up environment variables**
   Copy the example environment file and update it with your configuration:

   ```bash
   cp .env.example .env
   ```

   Then update the `.env` file with your database credentials and other settings.

### Available Make Commands

- `make migrate MIGRATION_NAME=Name` - Create a new EF Core migration
- `make db-update` - Apply pending database migrations
- `make remove-migrations` - Remove the most recent migration
- `make docker-up` - Start development containers (PostgreSQL and API)
- `make docker-down` - Stop and remove development containers
- `make docker-logs` - View container logs

3. **Run the application**

   ```bash
   make docker-up
   ```

> API Access The API will be available at `https://localhost:5053`

4. **Database Migrations (just for development purposes)**

   - Create a new migration:

     ```bash
     make migrate MIGRATION_NAME=YourMigrationName
     ```

   - Apply pending migrations:

     ```bash
     make db-update
     ```

   - Remove the last migration (if needed):

     ```bash
     make remove-migrations
     ```

### Running Without Make

#### Start Database

```bash
docker-compose -f docker-compose.yml up -d
```

#### Run Migrations

```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```

#### Run the Application

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run --project Presentation
```

## API Documentation

Once the application is running, you can access:

- Swagger UI: `https://localhost:5053/swagger`
- OpenAPI JSON: `https://localhost:5053/swagger/v1/swagger.json`

## Project Structure

The solution follows Clean Architecture principles with these main projects:

```
backend/
├── Application/       # Use cases, commands, handlers, DTOs
├── Domain/            # Business entities and interfaces
├── Infrastructure/    # Repositories, DB context, scripts
│   └── scripts/       # Migration/update shell scripts
├── Presentation/      # Endpoints, middlewares, program entry
├── SharedKernel/      # Common base classes/utilities
└── UnitTests/         # xUnit and NSubstitute-based tests
```

## Development

> **Note:** The `Makefile` is located in the `backend` folder. Make sure to run the commands from there.

### Running Tests

```bash
dotnet test
```

### Environment Configuration

The following environment variables can be set in the `.env` file:

- `CONNECTION_STRING`: Database connection string
- `RUN_MIGRATIONS`: Run database migrations (true/false)
- `ASPNETCORE_ENVIRONMENT`: Application environment (Development/Production)

## License

This project is licensed under the MIT License.

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request
