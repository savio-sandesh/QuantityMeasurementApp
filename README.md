# Quantity Measurement App

A layered .NET 10 console application for quantity comparison, conversion, and arithmetic across multiple measurement categories.

The solution is implemented through UC1 to UC18 and includes:
- A generic quantity engine with strong type safety.
- Category-specific unit adapters for Length, Weight, Volume, and Temperature.
- A layered application flow (UI -> Controller -> Business -> Repository).
- Configurable persistence (in-memory cache or SQL Server).
- Measurement history query support.

## Technology Stack
- .NET SDK 10.0+
- C# (nullable enabled)
- SQL Server (optional, for database mode)
- ASP.NET Core Web API (in `src/QuantityMeasurementWebApi`)
- Entity Framework Core SQL Server provider (`Microsoft.EntityFrameworkCore.SqlServer`)

## Current Implementation Status (UC1 to UC18)

### UC1 to UC8: Length Foundations and Arithmetic
- Length equality and inequality behavior.
- Multi-unit length support: Feet, Inch, Yard, Centimeter.
- Conversion APIs: static Convert and instance ConvertTo.
- Addition support with default and explicit target unit behavior.
- Refactoring for cleaner separation of conversion logic.

### UC9: Weight Category
- Added Weight with units Kilogram, Gram, Pound.
- Cross-unit conversion and arithmetic support consistent with length.

### UC10: Generic Quantity Engine
- Introduced generic Quantity<TUnit> with measurable adapters.
- Centralized conversion model with compile-time type safety.

### UC11: Volume Category
- Added Volume with units Litre, Millilitre, Gallon.
- Integrated into the generic quantity architecture.

### UC12 and UC13: Extended Arithmetic and DRY Refactor
- Added Subtract and Divide operations.
- Centralized arithmetic dispatch and validation logic.

### UC14: Temperature + Capability Constraints
- Added Temperature with units Celsius, Fahrenheit, Kelvin.
- Implemented non-linear conversion formulas.
- Restricted unsupported arithmetic for temperature with explicit exceptions.

### UC15: Layered Application + Persistence
- Implemented full layered flow with DTO/Entity separation.
- Added repository abstraction for cache and database storage.
- Startup behavior supports automatic database initialization in database mode.

### UC16: Measurement History Query Operations
- Repository contract supports:
	- GetAllMeasurements
	- GetByOperation
	- GetByType
	- GetCount
	- DeleteAll
- Implemented consistently in both cache and database repositories.

### UC17: REST API + Global Exception Handling
- Added ASP.NET Core Web API endpoints under `/api/v1/quantities`.
- Added API DTO contracts for request and response payloads.
- Added centralized global exception middleware returning structured `ErrorResponse` JSON.
- Added xUnit API test project setup with Moq, ASP.NET Core MVC Testing, and FluentAssertions.

### UC18: Local JWT Authentication (No Google/Firebase)
- Added local authentication with register and login endpoints under `/api/v1/auth`.
- Added `UserEntity` persistence model (`Users` table) with fields: Id, FullName, Email, PasswordHash, Role.
- Added BCrypt password hashing for secure password storage.
- Added JWT token creation with claims: NameIdentifier (Id), Email, and Role.
- Secured quantity endpoints using JWT bearer authentication and `[Authorize]`.
- Swagger UI now includes Bearer JWT authorization support with the `Authorize` button.
- Measurements now persist `UserId` from JWT claim (`ClaimTypes.NameIdentifier`) for Add and Compare operations.

## Architecture

### Layers
- UI Layer: Console menu and input validation.
- Controller Layer: Orchestrates user actions and service calls.
- Business Layer: Quantity operations, validation, and mapping to persistence entities.
- Repository Layer: Data access abstraction and concrete cache/database repositories.
- Model Layer: DTOs and persistence entities.
- Domain Layer: Core quantity model, unit enums, measurable adapters, and registry.

### Runtime Flow
1. User selects category and operation from console UI.
2. Controller forwards request to business service.
3. Service executes domain operation and persists operation metadata.
4. Repository stores data in cache or SQL database based on configuration.

## Project Structure

```text
src/
	QuantityMeasurementDomain/    # Core domain (Quantity<TUnit>, units, conversions)
	ModelLayer/                   # DTOs and entities
	RepositoryLayer/              # Repository interface, cache/db implementations, config
	BusinessLayer/                # Service, operation dispatch, entity factory
	QuantityMeasurementApp/       # Console UI, controller, entry point
	QuantityMeasurementWebApi/    # Web API host project (includes EF Core DbContext)
	QuantityMeasurement.Tests/    # xUnit test project for Web API and service-layer testing
test/
	QuantityMeasurementApp.Tests/ # Automated tests
```

Web API model contracts include request/response DTOs in `src/QuantityMeasurementWebApi/Models`:
- `QuantityDTO`
- `QuantityInputDTO`
- `QuantityMeasurementDTO`

Authentication DTO contracts are defined in `src/ModelLayer/DTO`:
- `UserRegisterDTO`
- `UserLoginDTO`

Web API endpoints are exposed under `/api/v1/quantities` for:
- `POST /compare`
- `POST /convert`
- `POST /add`
- `POST /subtract`
- `POST /divide`
- `GET /history/operation/{operationType}`
- `GET /history/type/{measurementType}`
- `GET /history` (returns only authenticated user's measurements)
- `GET /count`

Authentication endpoints are exposed under `/api/v1/auth`:
- `POST /register`
- `POST /login`

All `/api/v1/quantities/*` endpoints require a valid bearer token from `POST /api/v1/auth/login`.

Web API uses centralized global exception handling middleware (`GlobalExceptionMiddleware`) that returns structured JSON error responses (`ErrorResponse`) for domain and unexpected exceptions.

## Current Implementation Status (UC1 to UC18)

| UC | Area | Changelog Summary |
| --- | --- | --- |
| UC1 | Length Equality | Added core length equality and inequality behavior. |
| UC2 | Length Units | Added multi-unit length support: Feet, Inch, Yard, Centimeter. |
| UC3 | Length Conversion | Added length conversion support across units. |
| UC4 | Length API Refinement | Improved conversion API usability and internal conversion flow. |
| UC5 | Length Addition | Added length addition for same and cross-unit values. |
| UC6 | Length Target Unit Addition | Added addition with explicit target unit selection. |
| UC7 | Length Cleanup | Refactored length logic for clearer separation and maintainability. |
| UC8 | Length Stabilization | Finalized length behavior and arithmetic consistency checks. |
| UC9 | Weight Category | Added Weight with Kilogram, Gram, Pound and aligned arithmetic/conversion behavior. |
| UC10 | Generic Quantity Engine | Introduced generic Quantity<TUnit> and centralized measurable adapter model. |
| UC11 | Volume Category | Added Volume with Litre, Millilitre, Gallon within generic quantity architecture. |
| UC12 | Extended Arithmetic | Added Subtract operation and shared arithmetic dispatch path. |
| UC13 | DRY Arithmetic Refactor | Added Divide and consolidated arithmetic validation/dispatch logic. |
| UC14 | Temperature Constraints | Added Celsius/Fahrenheit/Kelvin with non-linear conversion and blocked unsupported arithmetic. |
| UC15 | Layered Persistence | Implemented DTO/entity layering, repository abstraction, and database-mode initialization path. |
| UC16 | History Query Operations | Added GetAllMeasurements, GetByOperation, GetByType, GetCount, DeleteAll in cache and database repositories. |
| UC17 | REST API + Global Error Handling | Added Web API endpoints, API DTOs, centralized GlobalExceptionMiddleware with ErrorResponse JSON, and API test project setup. |
| UC18 | Local JWT Authentication | Added UserEntity persistence, BCrypt hashing, register/login endpoints, JWT claim token generation, and `[Authorize]` protection for quantity APIs. |

## Configuration

Configuration file:
- src/RepositoryLayer/appsettings.json
- src/QuantityMeasurementWebApi/appsettings.json

Key settings:
- RepositoryType: database or cache
- ConnectionStrings.DefaultConnection: SQL Server connection string
- Web API EF Core also uses ConnectionStrings.DefaultConnection for QuantityDbContext
- Jwt.Key: HMAC secret key used for signing and validating JWTs
- Jwt.Issuer: token issuer value
- Jwt.Audience: token audience value

Behavior:
- If RepositoryType is database, startup runs database initialization.
- If RepositoryType is cache, data is kept in memory for the process lifetime.

## Build, Run, and Test

Run commands from repository root.

Build solution:

```bash
dotnet build .\QuantityMeasurementApp.slnx
```

Run application:

```bash
dotnet run --project .\src\QuantityMeasurementApp\QuantityMeasurementApp.csproj
```

Run full test suite:

```bash
dotnet test .\test\QuantityMeasurementApp.Tests\QuantityMeasurementApp.Tests.csproj
```

Latest verified result:
- 121 tests passed.

Additional API testing dependencies are configured in `src/QuantityMeasurement.Tests`:
- `Moq`
- `Microsoft.AspNetCore.Mvc.Testing`
- `FluentAssertions`

## Console Capabilities

### Categories
- Length
- Weight
- Volume
- Temperature

### Operations
- Equality
- Conversion
- Addition
- Subtraction
- Division

Temperature supports conversion and equality; unsupported arithmetic is intentionally blocked.

Business service API now returns `QuantityMeasurementDTO` for operation endpoints (compare, convert, add, subtract, divide), and supports history/query operations:
- `GetOperationHistory(string operation)`
- `GetMeasurementsByType(string type)`
- `GetOperationCount(string operation)`
- `GetErroredOperations()`

## Repository and Data Notes

Database repository supports transactional save behavior to protect data consistency.
Database repository operations now use EF Core (`QuantityDbContext`) and LINQ queries instead of raw ADO.NET commands.

History records store:
- Operand values and units
- Measurement type
- Operation name
- Result value and result unit
- CreatedAt timestamp initialized at entity construction time

Helper SQL script:
- src/RepositoryLayer/SqlTest.sql

## Quality and Maintainability Notes

Recent clean-code refactors include:
- Centralized constants for measurement types, operations, repository mode, and SQL query text.
- Standardized case-insensitive comparisons.
- Centralized measurement type dispatch logic.
- Centralized repository exception wrapping.
- Centralized entity creation for operation persistence records.
- EF-ready `QuantityMeasurementEntity` annotations for table/column/key mapping (`Measurements`, `Id`, `Value1`, `Unit1`, `Value2`, `Unit2`, `Result`, `OperationType`, `MeasurementType`, `CreatedAt`).


