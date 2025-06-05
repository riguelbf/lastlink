# lastlink Project

## Overview
This project is a full-stack application built using a backend in C# and a frontend in React. The project aims to provide an inventory management system.

This project provides a comprehensive inventory management system that allows users to manage products, track stock levels, and handle pricing. It features a user-friendly interface built with [React](https://reactjs.org/) and a robust backend developed in [C#](https://docs.microsoft.com/en-us/dotnet/csharp/). The application is designed to be easily deployable using [Docker](https://www.docker.com/) and [Docker Compose](https://docs.docker.com/compose/), facilitating a smooth development and production experience. It features a user-friendly interface built with React and a robust backend developed in C#. The application is designed to be easily deployable using Docker and Docker Compose, facilitating a smooth development and production experience.

### Prerequisites
- Docker
- Docker Compose
- Node JS
- .NET 9

### To-do list (for improvements)
- Implement user authentication and authorization.
- Add e2e tests for critical components.
- Add cache
- Optimize database queries for performance.
- Enhance the user interface for better UX.

## Development Environment
This project uses a dev-container to make development easier. It ensures that everyone has the same setup, which helps avoid issues. For more information, check out the [dev-container documentation](https://code.visualstudio.com/docs/remote/containers).


### Running the Application with Docker Compose
1. Clone the repository:
   ```bash
   git clone git@github.com:riguelbf/lastlink.git
   cd lastlink
   ```

2. Ensure you have the necessary environment variables set in your `.env` files for both backend and frontend.

3. Navigate to the project root directory and run:
   ```bash
   docker-compose up --build
   ```

4. Access the application in your browser at `http://localhost:5053`.

### Backend Documentation
- The backend is located in the `./backend` directory. It includes the API endpoints and business logic for the application.
- Refer to the `README.md` in the [`./backend`](https://github.com/riguelbf/lastlink/blob/main/backend/README.md) directory for detailed information on the backend setup and usage.

### Frontend Documentation
- The frontend is located in the `./frontend` directory. It includes the React application for the user interface.
- Refer to the `README.md` in the [`./frontend`](https://github.com/riguelbf/lastlink/blob/main/frontend/README.md) directory for detailed information on the frontend setup and usage.

## Additional References
- [React](https://reactjs.org/)
- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Serilog](https://serilog.net/)
- [Vite](https://vitejs.dev/)

## License
This project is licensed under the MIT License.