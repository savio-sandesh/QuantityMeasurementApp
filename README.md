# QuantityMeasurementApp

Small .NET sample: length and weight quantities with multi-unit arithmetic and conversions.

## Prerequisites
- .NET SDK 10.0 or later
- Run commands from repository root: `c:\QuantityMeasurementApp`

## Features
- `Length` supports equality, conversion, and addition across units (`Feet`, `Inch`, `Yard`, `Centimeter`).
- `Weight` supports equality, conversion, and addition across units (`Kilogram`, `Gram`, `Pound`).
- `Volume` supports equality, conversion, and addition across units (`Litre`, `Millilitre`, `Gallon`).
- `Quantity<TUnit>` provides a single generic implementation for equality, conversion, and addition across supported categories.
- `Quantity<TUnit>` now supports subtraction and division operations in addition to equality, conversion, and addition.
- Automatic unit conversion for arithmetic through base-unit normalization.
- Result in first operand's unit for default `Add(...)` behavior.
- Explicit target-unit addition overloads.
- Conversion API: static `Convert()` and instance `ConvertTo()` methods.
- Tolerance-based equality and normalized `GetHashCode()`.

## UC-wise Implementation

### Implemented (UC1) - Basic Equality for Same Unit

Files:
- `QuantityMeasurementApp/LengthInFeet.cs`
- `QuantityMeasurementApp/LengthInInch.cs`
- `QuantityMeasurementApp.Tests/LengthTests.cs`

Summary:
- Establishes foundational same-unit equality behavior for length values.

### Implemented (UC2) - Basic Inequality and Null Safety

Files:
- `QuantityMeasurementApp/LengthInFeet.cs`
- `QuantityMeasurementApp/LengthInInch.cs`
- `QuantityMeasurementApp.Tests/LengthTests.cs`

Summary:
- Adds non-equality and null-comparison behavior for baseline length objects.

### Implemented (UC3) - Generic Length Model

Files:
- `QuantityMeasurementApp/Length.cs`
- `QuantityMeasurementApp/LengthUnit.cs`
- `QuantityMeasurementApp.Tests/LengthTests.cs`

Summary:
- Implements generic `Length` value object using unit + value.
- Uses base-unit normalization and tolerance-based equality for cross-unit comparison.

### Implemented (UC4) - Extended Length Units

Files:
- `QuantityMeasurementApp/Length.cs`
- `QuantityMeasurementApp/LengthUnit.cs`
- `QuantityMeasurementApp.Tests/LengthTests.cs`

Summary:
- Adds support for `Yard` and `Centimeter` alongside `Feet` and `Inch`.
- Defines conversion factors relative to base unit (`Feet`).

### Implemented (UC5) - Unit-to-Unit Conversion API

Files:
- `QuantityMeasurementApp/Length.cs`
- `QuantityMeasurementApp.Tests/LengthTests.cs`

Summary:
- Static conversion API: `Length.Convert(value, source, target)`.
- Instance conversion API: `length.ConvertTo(targetUnit)`.
- Tests cover accuracy, round-trip conversion, and finite-value validation.

### Implemented (UC6) - Addition of Quantities

Files:
- `QuantityMeasurementApp/Length.cs`
- `QuantityMeasurementApp/Program.cs`
- `QuantityMeasurementApp.Tests/LengthTests.cs`

Summary:
- Adds default addition (`first.Add(second)`) returning result in first operand unit.
- Supports same-unit and cross-unit arithmetic.
- Includes null validation and edge-case coverage.

### Implemented (UC7) - Addition with Explicit Target Unit

Files:
- `QuantityMeasurementApp/Length.cs`
- `QuantityMeasurementApp/Program.cs`
- `QuantityMeasurementApp.Tests/LengthTests.cs`

Summary:
- Adds `Add(other, targetUnit)` and static target-unit addition overloads.
- Supports cross-unit addition with caller-selected result unit.
- Tests verify explicit-target addition and correctness under mixed units.

### Implemented (UC8) - Refactor Length for Cleaner Responsibilities

Files:
- `QuantityMeasurementApp/Length.cs`
- `QuantityMeasurementApp/LengthUnit.cs`
- `QuantityMeasurementApp/Program.cs`
- `QuantityMeasurementApp.Tests/LengthTests.cs`

Summary:
- Keeps `Length` focused on value-object behavior (equality, conversion, arithmetic).
- Delegates unit conversion factors and transformation methods to `LengthUnit` extensions.
- Validates refactored behavior through unit tests for equality, conversion, and addition.

### Implemented (UC9) - Replicate Length Pattern for Weight

Files:
- `QuantityMeasurementApp/Weight.cs`
- `QuantityMeasurementApp/WeightUnit.cs`
- `QuantityMeasurementApp/Program.cs`
- `QuantityMeasurementApp.Tests/WeightTests.cs`

Summary:
- Implements weight quantities with multi-unit arithmetic and conversions.
- Supports addition across units (`Kilogram`, `Gram`, `Pound`).
- Automatic conversion for arithmetic through base unit (`Kilogram`).
- Provides static `Weight.Convert(...)` and instance `ConvertTo(...)` APIs.
- Includes tolerance-based equality, consistent `GetHashCode()`, and explicit target-unit addition.
- Ensures category separation (`Weight` is not equal to `Length`).

### Implemented (UC10) - Generic Quantity Class with Unit Interface

Files:
- `QuantityMeasurementApp/IMeasurableUnit.cs`
- `QuantityMeasurementApp/MeasurableRegistry.cs`
- `QuantityMeasurementApp/LengthUnitMeasurable.cs`
- `QuantityMeasurementApp/WeightUnitMeasurable.cs`
- `QuantityMeasurementApp/Quantity.cs`
- `QuantityMeasurementApp/Program.cs`
- `QuantityMeasurementApp.Tests/QuantityTests.cs`

Summary:
- Introduces one generic class `Quantity<TUnit>` to replace duplicated category-specific quantity logic.
- Uses a shared measurable contract (`IMeasurableUnit<TUnit>`) for conversion behavior.
- Preserves compile-time type safety through generics (`Quantity<LengthUnit>` is not `Quantity<WeightUnit>`).
- Adds generic demonstration methods in `Program` and a UC10 demo menu option.
- Keeps UC1-UC9 classes and tests operational while adding UC10 architecture.

### Implemented (UC11) - Volume Measurement with Generic Quantity

Files:
- `QuantityMeasurementApp/VolumeUnit.cs`
- `QuantityMeasurementApp/VolumeUnitMeasurable.cs`
- `QuantityMeasurementApp/MeasurableRegistry.cs`
- `QuantityMeasurementApp/Program.cs`
- `QuantityMeasurementApp.Tests/QuantityTests.cs`

Summary:
- Adds a third category (`VolumeUnit`) using the existing generic `Quantity<TUnit>` architecture.
- Conversion factors are relative to litre as base unit: `Litre=1.0`, `Millilitre=0.001`, `Gallon=3.78541`.
- No changes were needed in `Quantity<TUnit>`; only adapter registration and tests were extended.
- Confirms scalability of UC10 architecture by adding a new category with minimal code changes.

### Implemented (UC12) - Subtraction and Division Operations

Files:
- `QuantityMeasurementApp/Quantity.cs`
- `QuantityMeasurementApp/Program.cs`
- `QuantityMeasurementApp.Tests/QuantityTests.cs`

Summary:
- Adds generic subtraction APIs: `Subtract(other)` and `Subtract(other, targetUnit)`.
- Adds generic division API: `Divide(other)` returning a dimensionless scalar ratio.
- Preserves immutability: arithmetic returns new quantity objects where applicable.
- Adds validation for null operands and divide-by-zero scenarios.
- Extends generic demo output to include subtraction and division examples for length, weight, and volume.

### Implemented (UC13) - Centralized Arithmetic Logic (DRY)

Files:
- `QuantityMeasurementApp/Quantity.cs`
- `QuantityMeasurementApp.Tests/QuantityTests.cs`

Summary:
- Refactors `Add`, `Subtract`, and `Divide` to delegate through centralized private helpers.
- Introduces internal arithmetic operation dispatch with a private `ArithmeticOperation` enum.
- Centralizes arithmetic operand validation in one method to remove duplicated checks.
- Preserves UC12 public API signatures and behavior while improving maintainability.

## Getting Started

Build:

```bash
dotnet build .\QuantityMeasurementApp.slnx
```

Run demo:

```bash
dotnet run --project .\QuantityMeasurementApp\QuantityMeasurementApp.csproj
```

Run tests:

```bash
dotnet test .\QuantityMeasurementApp.Tests\QuantityMeasurementApp.Tests.csproj
```

## Console Menu
- `1` Compare two lengths
- `2` Convert length units
- `3` Add two lengths
- `4` Compare two weights
- `5` Convert weight units
- `6` Add two weights
- `7` Run generic quantity demo (UC10+)
- Option `7` now demonstrates generic equality, conversion, addition, subtraction, and division for length, weight, and volume (UC10-UC13 flow).

## Notes on Naming
- In this C# codebase, Java-style names like `QuantityLength`/`QuantityWeight` are represented by `Length`/`Weight`.
- Functionality is equivalent to the UC intent.
- Java-style enum-interface implementation is adapted to C# using measurable adapters because C# enums cannot implement custom interfaces.

## Legacy Compatibility Classes
- `QuantityMeasurementApp/LengthInFeet.cs` and `QuantityMeasurementApp/LengthInInch.cs` remain in the repository from earlier incremental steps.
- Current UC3+ architecture is centered on `Length` and `LengthUnit`.

## README Maintenance Rule
- For every future change affecting behavior, API, units, or tests, update this `README.md` in the same change.
