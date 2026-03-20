using System;
using ModelLayer;
using QuantityMeasurementDomain;
using QuantityMeasurementDomain.Units;
using RepositoryLayer;
using UtilityLayer;

namespace BusinessLayer
{
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository repository;

        public QuantityMeasurementService(IQuantityMeasurementRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public QuantityMeasurementService() : this(GetRepository()) { }

        private static IQuantityMeasurementRepository GetRepository()
        {
            var repoType = DatabaseConfig.GetRepositoryType();

            if (repoType.Equals("database", StringComparison.OrdinalIgnoreCase))
                return new QuantityMeasurementDatabaseRepository();

            return QuantityMeasurementCacheRepository.Instance;
        }

        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            Logger.Info("Comparing quantities");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            bool result = a.Equals(b);

            var entity = new QuantityMeasurementEntity
            {
                FirstValue = q1.Value,
                FirstUnit = q1.Unit,
                FirstMeasurementType = q1.MeasurementType,
                SecondValue = q2.Value,
                SecondUnit = q2.Unit,
                ResultValue = result.ToString(),
                ResultUnit = string.Empty,
                Operation = "COMPARE",
                IsError = false,
                ErrorMessage = string.Empty
            };

            repository.Save(entity);

            Logger.Debug($"Compare result: {result}");

            return result;
        }

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            Logger.Info("Performing ADD operation");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            var result = a.Add(b, ParseTargetUnit(targetUnit, q1.MeasurementType));

            var entity = new QuantityMeasurementEntity
            {
                FirstValue = q1.Value,
                FirstUnit = q1.Unit,
                FirstMeasurementType = q1.MeasurementType,
                SecondValue = q2.Value,
                SecondUnit = q2.Unit,
                ResultValue = result.Value.ToString(),
                ResultUnit = targetUnit,
                Operation = "ADD",
                IsError = false,
                ErrorMessage = string.Empty
            };

            repository.Save(entity);

            Logger.Debug($"Add result: {result.Value}");

            return CreateDTO(result, q1.MeasurementType);
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            Logger.Info("Performing SUBTRACT operation");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            var result = a.Subtract(b, ParseTargetUnit(targetUnit, q1.MeasurementType));

            var entity = new QuantityMeasurementEntity
            {
                FirstValue = q1.Value,
                FirstUnit = q1.Unit,
                FirstMeasurementType = q1.MeasurementType,
                SecondValue = q2.Value,
                SecondUnit = q2.Unit,
                ResultValue = result.Value.ToString(),
                ResultUnit = targetUnit,
                Operation = "SUBTRACT",
                IsError = false,
                ErrorMessage = string.Empty
            };

            repository.Save(entity);

            Logger.Debug($"Subtract result: {result.Value}");

            return CreateDTO(result, q1.MeasurementType);
        }

        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            Logger.Info("Performing DIVIDE operation");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            double result = a.Divide(b);

            var entity = new QuantityMeasurementEntity
            {
                FirstValue = q1.Value,
                FirstUnit = q1.Unit,
                FirstMeasurementType = q1.MeasurementType,
                SecondValue = q2.Value,
                SecondUnit = q2.Unit,
                ResultValue = result.ToString(),
                ResultUnit = string.Empty,
                Operation = "DIVIDE",
                IsError = false,
                ErrorMessage = string.Empty
            };

            repository.Save(entity);

            Logger.Debug($"Divide result: {result}");

            return result;
        }

        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            Logger.Info("Performing CONVERT operation");

            dynamic q = CreateQuantity(quantity);

            var result = ConvertToTarget(q, targetUnit, quantity.MeasurementType);

            var entity = new QuantityMeasurementEntity
            {
                FirstValue = quantity.Value,
                FirstUnit = quantity.Unit,
                FirstMeasurementType = quantity.MeasurementType,
                SecondValue = 0,
                SecondUnit = targetUnit,
                ResultValue = result.Value.ToString(),
                ResultUnit = targetUnit,
                Operation = "CONVERT",
                IsError = false,
                ErrorMessage = string.Empty
            };

            repository.Save(entity);

            Logger.Debug($"Convert result: {result.Value}");

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