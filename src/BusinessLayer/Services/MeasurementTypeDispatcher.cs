using ModelLayer;
using QuantityMeasurementDomain;
using QuantityMeasurementDomain.Units;
using System;

namespace BusinessLayer
{
    internal static class MeasurementTypeDispatcher
    {
        public static dynamic ParseTargetUnit(string unit, string measurementType)
        {
            string normalizedType = NormalizeMeasurementType(measurementType);
            string normalizedUnit = NormalizeUnit(unit, "target unit");

            if (IsType(normalizedType, MeasurementTypeConstants.Length))
                return ParseUnit<LengthUnit>(normalizedUnit, normalizedType, "target unit");

            if (IsType(normalizedType, MeasurementTypeConstants.Weight))
                return ParseUnit<WeightUnit>(normalizedUnit, normalizedType, "target unit");

            if (IsType(normalizedType, MeasurementTypeConstants.Volume))
                return ParseUnit<VolumeUnit>(normalizedUnit, normalizedType, "target unit");

            if (IsType(normalizedType, MeasurementTypeConstants.Temperature))
                return ParseUnit<TemperatureUnit>(normalizedUnit, normalizedType, "target unit");

            throw UnsupportedMeasurementType(normalizedType);
        }

        public static dynamic CreateQuantity(QuantityDTO dto)
        {
            string normalizedType = NormalizeMeasurementType(dto.MeasurementType);
            string normalizedUnit = NormalizeUnit(dto.Unit, "unit");

            if (IsType(normalizedType, MeasurementTypeConstants.Length))
                return CreateQuantity<LengthUnit>(dto.Value, normalizedUnit, normalizedType);

            if (IsType(normalizedType, MeasurementTypeConstants.Weight))
                return CreateQuantity<WeightUnit>(dto.Value, normalizedUnit, normalizedType);

            if (IsType(normalizedType, MeasurementTypeConstants.Volume))
                return CreateQuantity<VolumeUnit>(dto.Value, normalizedUnit, normalizedType);

            if (IsType(normalizedType, MeasurementTypeConstants.Temperature))
                return CreateQuantity<TemperatureUnit>(dto.Value, normalizedUnit, normalizedType);

            throw UnsupportedMeasurementType(normalizedType);
        }

        public static dynamic ConvertToTarget(dynamic quantity, string targetUnit, string measurementType)
        {
            string normalizedType = NormalizeMeasurementType(measurementType);
            string normalizedTargetUnit = NormalizeUnit(targetUnit, "target unit");

            if (IsType(normalizedType, MeasurementTypeConstants.Length))
                return ConvertTo<LengthUnit>(quantity, normalizedTargetUnit, normalizedType);

            if (IsType(normalizedType, MeasurementTypeConstants.Weight))
                return ConvertTo<WeightUnit>(quantity, normalizedTargetUnit, normalizedType);

            if (IsType(normalizedType, MeasurementTypeConstants.Volume))
                return ConvertTo<VolumeUnit>(quantity, normalizedTargetUnit, normalizedType);

            if (IsType(normalizedType, MeasurementTypeConstants.Temperature))
                return ConvertTo<TemperatureUnit>(quantity, normalizedTargetUnit, normalizedType);

            throw UnsupportedMeasurementType(normalizedType);
        }

        private static bool IsType(string type, string expected)
        {
            return string.Equals(type, expected, StringComparison.OrdinalIgnoreCase);
        }

        private static TUnit ParseUnit<TUnit>(string unit, string measurementType, string unitLabel)
            where TUnit : struct, Enum
        {
            if (!Enum.TryParse<TUnit>(unit, true, out var parsedUnit))
            {
                throw InvalidUnitForType(unitLabel, unit, measurementType, GetSupportedUnits<TUnit>());
            }

            return parsedUnit;
        }

        private static Quantity<TUnit> CreateQuantity<TUnit>(double value, string unit, string measurementType)
            where TUnit : struct, Enum
        {
            if (!Enum.TryParse<TUnit>(unit, true, out var parsedUnit))
            {
                throw InvalidUnitForType("unit", unit, measurementType, GetSupportedUnits<TUnit>());
            }

            return new Quantity<TUnit>(value, parsedUnit);
        }

        private static dynamic ConvertTo<TUnit>(dynamic quantity, string targetUnit, string measurementType)
            where TUnit : struct, Enum
        {
            if (!Enum.TryParse<TUnit>(targetUnit, true, out var parsedUnit))
            {
                throw InvalidUnitForType("target unit", targetUnit, measurementType, GetSupportedUnits<TUnit>());
            }

            return quantity.ConvertTo(parsedUnit);
        }

        private static string NormalizeMeasurementType(string measurementType)
        {
            if (string.IsNullOrWhiteSpace(measurementType))
            {
                throw new QuantityMeasurementException("Measurement type is required.");
            }

            return measurementType.Trim();
        }

        private static string NormalizeUnit(string unit, string unitLabel)
        {
            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new QuantityMeasurementException($"{ToTitleCase(unitLabel)} is required.");
            }

            return unit.Trim();
        }

        private static QuantityMeasurementException UnsupportedMeasurementType(string measurementType)
        {
            string supportedTypes = string.Join(", ", MeasurementTypeConstants.Length, MeasurementTypeConstants.Weight, MeasurementTypeConstants.Volume, MeasurementTypeConstants.Temperature);
            return new QuantityMeasurementException($"Unsupported measurement type '{measurementType}'. Supported types: {supportedTypes}.");
        }

        private static QuantityMeasurementException InvalidUnitForType(string unitLabel, string unit, string measurementType, string supportedUnits)
        {
            return new QuantityMeasurementException($"Invalid {unitLabel} '{unit}' for measurement type '{measurementType}'. Supported units: {supportedUnits}.");
        }

        private static string GetSupportedUnits<TUnit>() where TUnit : struct, Enum
        {
            return string.Join(", ", Enum.GetNames(typeof(TUnit)));
        }

        private static string ToTitleCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }
    }
}