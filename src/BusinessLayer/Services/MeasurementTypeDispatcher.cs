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
            if (IsType(measurementType, MeasurementTypeConstants.Length))
                return ParseUnit<LengthUnit>(unit);

            if (IsType(measurementType, MeasurementTypeConstants.Weight))
                return ParseUnit<WeightUnit>(unit);

            if (IsType(measurementType, MeasurementTypeConstants.Volume))
                return ParseUnit<VolumeUnit>(unit);

            if (IsType(measurementType, MeasurementTypeConstants.Temperature))
                return ParseUnit<TemperatureUnit>(unit);

            throw new QuantityMeasurementException("Unsupported measurement type");
        }

        public static dynamic CreateQuantity(QuantityDTO dto)
        {
            if (IsType(dto.MeasurementType, MeasurementTypeConstants.Length))
                return CreateQuantity<LengthUnit>(dto, "length");

            if (IsType(dto.MeasurementType, MeasurementTypeConstants.Weight))
                return CreateQuantity<WeightUnit>(dto, "weight");

            if (IsType(dto.MeasurementType, MeasurementTypeConstants.Volume))
                return CreateQuantity<VolumeUnit>(dto, "volume");

            if (IsType(dto.MeasurementType, MeasurementTypeConstants.Temperature))
                return CreateQuantity<TemperatureUnit>(dto, "temperature");

            throw new QuantityMeasurementException($"Unsupported measurement type: {dto.MeasurementType}");
        }

        public static dynamic ConvertToTarget(dynamic quantity, string targetUnit, string measurementType)
        {
            if (IsType(measurementType, MeasurementTypeConstants.Length))
                return ConvertTo<LengthUnit>(quantity, targetUnit);

            if (IsType(measurementType, MeasurementTypeConstants.Weight))
                return ConvertTo<WeightUnit>(quantity, targetUnit);

            if (IsType(measurementType, MeasurementTypeConstants.Volume))
                return ConvertTo<VolumeUnit>(quantity, targetUnit);

            if (IsType(measurementType, MeasurementTypeConstants.Temperature))
                return ConvertTo<TemperatureUnit>(quantity, targetUnit);

            throw new QuantityMeasurementException($"Unsupported measurement type: {measurementType}");
        }

        private static bool IsType(string type, string expected)
        {
            return string.Equals(type, expected, StringComparison.OrdinalIgnoreCase);
        }

        private static TUnit ParseUnit<TUnit>(string unit) where TUnit : struct, Enum
        {
            if (!Enum.TryParse<TUnit>(unit, true, out var parsedUnit))
                throw new QuantityMeasurementException($"Invalid target unit: {unit}");

            return parsedUnit;
        }

        private static Quantity<TUnit> CreateQuantity<TUnit>(QuantityDTO dto, string typeLabel)
            where TUnit : struct, Enum
        {
            if (!Enum.TryParse<TUnit>(dto.Unit, true, out var parsedUnit))
                throw new QuantityMeasurementException($"Invalid {typeLabel} unit: {dto.Unit}");

            return new Quantity<TUnit>(dto.Value, parsedUnit);
        }

        private static dynamic ConvertTo<TUnit>(dynamic quantity, string targetUnit)
            where TUnit : struct, Enum
        {
            if (!Enum.TryParse<TUnit>(targetUnit, true, out var parsedUnit))
                throw new QuantityMeasurementException($"Invalid target unit: {targetUnit}");

            return quantity.ConvertTo(parsedUnit);
        }
    }
}