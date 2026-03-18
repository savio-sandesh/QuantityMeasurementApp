using System;
using ModelLayer;
using QuantityMeasurementDomain;
using QuantityMeasurementDomain.Units;
using RepositoryLayer;

namespace BusinessLayer
{
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository repository;

        public QuantityMeasurementService(IQuantityMeasurementRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public QuantityMeasurementService() : this(QuantityMeasurementCacheRepository.Instance) { }

        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            return a.Equals(b);
        }

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            var result = a.Add(b, ParseTargetUnit(targetUnit, q1.MeasurementType));

            return CreateDTO(result, q1.MeasurementType);
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            var result = a.Subtract(b, ParseTargetUnit(targetUnit, q1.MeasurementType));

            return CreateDTO(result, q1.MeasurementType);
        }

        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            return a.Divide(b);
        }

        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            dynamic q = CreateQuantity(quantity);

            var result = ConvertToTarget(q, targetUnit, quantity.MeasurementType);

            return CreateDTO(result, quantity.MeasurementType);
        }

        // ---------------- HELPERS ----------------

        private static void ValidateType(QuantityDTO q1, QuantityDTO q2)
        {
            if (q1.MeasurementType != q2.MeasurementType)
                throw new QuantityMeasurementException("Different measurement types cannot be compared");
        }


        private static dynamic ParseTargetUnit(string unit, string type)
        {
            return type.ToLower() switch
            {
                "length" => Enum.Parse<LengthUnit>(unit, true),
                "weight" => Enum.Parse<WeightUnit>(unit, true),
                "volume" => Enum.Parse<VolumeUnit>(unit, true),
                "temperature" => Enum.Parse<TemperatureUnit>(unit, true),
                _ => throw new QuantityMeasurementException("Unsupported measurement type")
            };
        }

        private static dynamic CreateQuantity(QuantityDTO dto)
        {
            switch (dto.MeasurementType.ToLower())
            {
                case "length":
                    if (!Enum.TryParse<LengthUnit>(dto.Unit, true, out var lUnit))
                        throw new QuantityMeasurementException($"Invalid length unit: {dto.Unit}");
                    return new Quantity<LengthUnit>(dto.Value, lUnit);

                case "weight":
                    if (!Enum.TryParse<WeightUnit>(dto.Unit, true, out var wUnit))
                        throw new QuantityMeasurementException($"Invalid weight unit: {dto.Unit}");
                    return new Quantity<WeightUnit>(dto.Value, wUnit);

                case "volume":
                    if (!Enum.TryParse<VolumeUnit>(dto.Unit, true, out var vUnit))
                        throw new QuantityMeasurementException($"Invalid volume unit: {dto.Unit}");
                    return new Quantity<VolumeUnit>(dto.Value, vUnit);

                case "temperature":
                    if (!Enum.TryParse<TemperatureUnit>(dto.Unit, true, out var tUnit))
                        throw new QuantityMeasurementException($"Invalid temperature unit: {dto.Unit}");
                    return new Quantity<TemperatureUnit>(dto.Value, tUnit);

                default:
                    throw new QuantityMeasurementException($"Unsupported measurement type: {dto.MeasurementType}");
            }
        }

        private static dynamic ConvertToTarget(dynamic quantity, string targetUnit, string type)
        {
            switch (type.ToLower())
            {
                case "length":
                    if (!Enum.TryParse<LengthUnit>(targetUnit, true, out var lUnit))
                        throw new QuantityMeasurementException($"Invalid target unit: {targetUnit}");
                    return quantity.ConvertTo(lUnit);

                case "weight":
                    if (!Enum.TryParse<WeightUnit>(targetUnit, true, out var wUnit))
                        throw new QuantityMeasurementException($"Invalid target unit: {targetUnit}");
                    return quantity.ConvertTo(wUnit);

                case "volume":
                    if (!Enum.TryParse<VolumeUnit>(targetUnit, true, out var vUnit))
                        throw new QuantityMeasurementException($"Invalid target unit: {targetUnit}");
                    return quantity.ConvertTo(vUnit);

                case "temperature":
                    if (!Enum.TryParse<TemperatureUnit>(targetUnit, true, out var tUnit))
                        throw new QuantityMeasurementException($"Invalid target unit: {targetUnit}");
                    return quantity.ConvertTo(tUnit);

                default:
                    throw new QuantityMeasurementException($"Unsupported measurement type: {type}");
            }
        }

        private static QuantityDTO CreateDTO(dynamic quantity, string type)
        {
            return new QuantityDTO(
                quantity.Value,
                quantity.Unit.ToString(),
                type
            );
        }
    }
}