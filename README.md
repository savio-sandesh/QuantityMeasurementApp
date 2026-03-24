# Quantity Measurement App

A layered .NET 10 console application for quantity comparison, conversion, and arithmetic across multiple measurement categories.

The solution is implemented through UC1 to UC16 and includes:
- A generic quantity engine with strong type safety.
- Category-specific unit adapters for Length, Weight, Volume, and Temperature.
- A layered application flow (UI -> Controller -> Business -> Repository).
- Configurable persistence (in-memory cache or SQL Server).
- Measurement history query support.

## Technology Stack
- .NET SDK 10.0+
- C# (nullable enabled)
- SQL Server (optional, for database mode)

## Current Implementation Status (UC1 to UC16)

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
test/
	QuantityMeasurementApp.Tests/ # Automated tests
```

## Configuration

Configuration file:
- src/RepositoryLayer/appsettings.json

Key settings:
- RepositoryType: database or cache
- ConnectionStrings.DefaultConnection: SQL Server connection string

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

## Repository and Data Notes

Database repository supports transactional save behavior to protect data consistency.

History records store:
- Operand values and units
- Measurement type
- Operation name
- Result value and result unit

Helper SQL script:
- src/RepositoryLayer/SqlTest.sql

## Quality and Maintainability Notes

Recent clean-code refactors include:
- Centralized constants for measurement types, operations, repository mode, and SQL query text.
- Standardized case-insensitive comparisons.
- Centralized measurement type dispatch logic.
- Centralized repository exception wrapping.
- Centralized entity creation for operation persistence records.

## README Maintenance Rule

For every future change affecting behavior, API, units, tests, architecture, or configuration, update this README in the same change.
