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
   git clone https://github.com/your-username/lastlink.git
   cd lastlink/backend
   ```

2. **Set up environment variables**
   Create a `.env` file in the `backend` directory with the following content:
   ```
   CONNECTION_STRING=Host=localhost;Database=lastlink;Username=postgres;Password=postgres
   FRONTEND_URL=http://localhost:3000
   ```

3. **Start the database**
   ```bash
   docker-compose up -d
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update --project Infrastructure
   ```

5. **Run the application**
   ```bash
   dotnet run --project Presentation
   ```

   The API will be available at `https://localhost:5053`

## API Documentation

Once the application is running, you can access the Swagger UI at `https://localhost:5053/swagger`

## Project Structure

The solution is organized into several projects following Clean Architecture principles:

- **Application**: Contains application logic, use cases, and DTOs
- **Domain**: Contains the domain model, entities, and repository interfaces
- **Infrastructure**: Contains data access, external services, and other infrastructure concerns
- **Presentation**: Contains the Web API controllers and API endpoints
- **SharedKernel**: Contains common base classes and interfaces used across layers

## Testing

To run the tests:

```bash
dotnet test
```

## Development

### Running Migrations

To create a new migration:

```bash
dotnet ef migrations add MigrationName --project Infrastructure --startup-project Presentation
```

To apply migrations:

```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```

## Deployment

### Docker

Build the Docker image:

```bash
docker build -t lastlink-backend .
```

Run the container:

```bash
docker run -d -p 5053:80 --name lastlink-backend \
  -e CONNECTION_STRING=your_connection_string \
  -e FRONTEND_URL=your_frontend_url \
  lastlink-backend
```

## License

This project is licensed under the MIT License.
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
