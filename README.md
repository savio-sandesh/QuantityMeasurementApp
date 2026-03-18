# QuantityMeasurementApp

Small .NET sample: length, weight, volume, and temperature quantities with conversion and type-safe operations.

## Prerequisites
- .NET SDK 10.0 or later
- Run commands from repository root: `c:\QuantityMeasurementApp`

## Features
- Core quantity types (`Length`, `Weight`, `Quantity<TUnit>`, and unit enums) are implemented in `QuantityMeasurementDomain`.
- The runnable console application is `src/QuantityMeasurementApp`.
- `Length` supports equality, conversion, and addition across units (`Feet`, `Inch`, `Yard`, `Centimeter`).
- `Weight` supports equality, conversion, and addition across units (`Kilogram`, `Gram`, `Pound`).
- `Volume` supports equality, conversion, and addition across units (`Litre`, `Millilitre`, `Gallon`).
- `Temperature` supports equality and conversion across units (`Celsius`, `Fahrenheit`, `Kelvin`).
- `Quantity<TUnit>` provides a single generic implementation for equality, conversion, and addition across supported categories.
- `Quantity<TUnit>` now supports subtraction and division operations in addition to equality, conversion, and addition.
- UC14 operation capability checks allow category-specific constraints (for example, temperature arithmetic is blocked for absolute values).
- Automatic unit conversion for arithmetic through base-unit normalization.
- Result in first operand's unit for default `Add(...)` behavior.
- Explicit target-unit addition overloads.
- Conversion API: static `Convert()` and instance `ConvertTo()` methods.
- Tolerance-based equality (Length, Weight, and generic Quantity use `0.000001`) and normalized `GetHashCode()`.
- Non-linear conversion support through measurable adapters (temperature formulas use offsets and scaling).

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

### Implemented (UC14) - Temperature + Selective Arithmetic Support

Files:
- `QuantityMeasurementApp/IMeasurableUnit.cs`
- `QuantityMeasurementApp/TemperatureUnit.cs`
- `QuantityMeasurementApp/TemperatureUnitMeasurable.cs`
- `QuantityMeasurementApp/UnsupportedOperationException.cs`
- `QuantityMeasurementApp/MeasurableRegistry.cs`
- `QuantityMeasurementApp/Quantity.cs`
- `QuantityMeasurementApp/Program.cs`
- `QuantityMeasurementApp.Tests/QuantityTests.cs`

Summary:
- Adds `TemperatureUnit` with `Celsius`, `Fahrenheit`, and `Kelvin`.
- Adds non-linear temperature conversion formulas via `TemperatureUnitMeasurable`.
- Refactors `IMeasurableUnit<TUnit>` with default capability methods for arithmetic support checks.
- Validates operation support in centralized arithmetic paths before add/subtract/divide execution.
- Blocks temperature arithmetic with clear `UnsupportedOperationException` messages.
- Preserves UC1-UC13 behavior for length, weight, and volume through backward-compatible defaults.

## Getting Started

Build:

```bash
dotnet build .\QuantityMeasurementApp.slnx
```

Run demo:

```bash
dotnet run --project .\src\QuantityMeasurementApp\QuantityMeasurementApp.csproj
```

Run tests:

```bash
dotnet test .\test\QuantityMeasurementApp.Tests\QuantityMeasurementApp.Tests.csproj
```

## Console Menu
- Category menu:
- `1` Length
- `2` Weight
- `3` Volume
- `4` Temperature
- `0` Exit
- Operation menu for Length/Weight/Volume:
- `1` Equality
- `2` Conversion
- `3` Addition
- `4` Subtraction
- `5` Division
- `6` Back (returns to category menu)
- Operation menu for Temperature:
- `1` Conversion
- `2` Equality
- `3` Back (returns to category menu)
- Unit guidance (shown before each unit input):
- Length: `Feet`, `Inch`, `Yard`, `Centimeter`
- Weight: `Kilogram`, `Gram`, `Pound`
- Volume: `Litre`, `Millilitre`, `Gallon`
- Temperature: `Celsius`, `Fahrenheit`, `Kelvin`
- Unit validation:
- Unit inputs must be unit names (case-insensitive).
- Numeric unit input (for example `1` or `2`) is rejected with an error.
- Menu validation:
- Category and operation selections are validated and re-prompted until a valid option is entered.

## Notes on Naming
- In this C# codebase, Java-style names like `QuantityLength`/`QuantityWeight` are represented by `Length`/`Weight`.
- Functionality is equivalent to the UC intent.
- Java-style enum-interface implementation is adapted to C# using measurable adapters because C# enums cannot implement custom interfaces.

## Legacy Compatibility Classes
- `QuantityMeasurementApp/LengthInFeet.cs` and `QuantityMeasurementApp/LengthInInch.cs` remain in the repository from earlier incremental steps.
- Current UC3+ architecture is centered on `Length` and `LengthUnit`.

## README Maintenance Rule
- For every future change affecting behavior, API, units, or tests, update this `README.md` in the same change.
