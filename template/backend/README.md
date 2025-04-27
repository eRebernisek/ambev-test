# Backend Setup Guide

## Prerequisites
- Docker installed on your system
- .NET SDK (version specified in the project)

### Connection Details
- Server: localhost
- Port: 5432
- Database: DeveloperEvaluation
- Username: postgres
- Password: postgres

### Notes
- The container runs on port 5432
- Make sure port 5432 is available on your system
- The container name is "ambev_test"
- The container runs in detached mode (-d flag)

## Initial Database Setup
The database is automatically initialized with the following:

### Default Tables
- **Users** table with columns:
  - Id (uuid, primary key)
  - Username (varchar(50))
  - Password (varchar(100))
  - Phone (varchar(20))
  - Email (varchar(100))
  - Status (varchar(20))
  - Role (varchar(20))

### Default User
A default admin user is created with the following credentials:
- Username: admin
- Password: Admin123
- Email: admin@email.com
- Phone: 1234567890
- Status: Active
- Role: Admin

## Authentication
To authenticate:
1. Use the `/api/auth/login` endpoint with the credentials above
2. Copy the JWT token from the response
3. Add the token to your requests in the Authorization header: `Bearer <token>`

## Do the following to run the project:

```bash
# Start PostgreSQL container with initial data
docker run -e POSTGRES_PASSWORD=postgres \
           -e POSTGRES_USER=postgres \
           -e POSTGRES_DB=DeveloperEvaluation \
           -p 5432:5432 \
           -v $(pwd)/init.sql:/docker-entrypoint-initdb.d/init.sql \
           --name ambev_test \
           -d postgres:latest

docker-compose up --build

# Build and run the application
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet build Ambev.DeveloperEvaluation.sln
dotnet run

# Run tests
dotnet test Ambev.DeveloperEvaluation.sln
```

## Development Notes
- The application uses Domain-Driven Design (DDD) principles
- Entity Framework Core is used for database operations
- The solution follows a clean architecture approach
- JWT authentication is used for securing the API endpoints
- All endpoints require authentication except for the login endpoint