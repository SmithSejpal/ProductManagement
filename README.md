# Product Management API

A C#/.NET application for managing product transactions with a scalable and well-structured design.

## Features
- Standard CRUD operations for products
- Auto-generated unique 6-digit product IDs (designed for a distributed environment)
- Stock management:
  - Decrement stock by a given quantity
  - Add stock by a given quantity
- Implements:
  - Repository and Unit of Work patterns
  - Logging and dependency injection
  - Concurrency handling
- Uses Entity Framework Core (EF Core) with Code-First Migrations
- Comprehensive unit tests for core functionalities

## Endpoints

| Method | Endpoint | Description |
|--------|---------|-------------|
| POST | `/api/products` | Create a new product |
| GET | `/api/products` | Get all products with stock details |
| GET | `/api/products/{id}` | Get product by ID |
| PUT | `/api/products/{id}` | Update an existing product |
| DELETE | `/api/products/{id}` | Delete a product |
| PUT | `/api/products/decrement-stock/{id}/{quantity}` | Decrease product stock |
| PUT | `/api/products/add-to-stock/{id}/{quantity}` | Increase product stock |

## Setup

### Prerequisites
- .NET 8 installed
- SQL Server (ensure the database connection settings are updated in `appsettings.json`)

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/SmithSejpal/ProductManagement.git
   ```
2. Navigate to the project folder:
   ```sh
   cd ProductManagement
   ```
3. Install dependencies:
   ```sh
   dotnet restore
   ```
4. Apply EF Core migrations:
   ```sh
   dotnet ef database update
   ```
5. Run the application:
   ```sh
   dotnet run
   ```

## Code Considerations
- **Product ID Generation:** Ensures uniqueness across multiple instances using a distributed-safe algorithm.
- **Logging:** Uses structured logging to track API calls and database transactions.
- **Unit Testing:** Includes test cases to validate core functionalities.

## Contribution
Feel free to fork and submit pull requests to enhance the project!

---
This project demonstrates a well-architected approach for handling product management within a scalable .NET application.

