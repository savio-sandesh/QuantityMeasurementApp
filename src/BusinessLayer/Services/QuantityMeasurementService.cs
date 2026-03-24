using System;
using ModelLayer;
using QuantityMeasurementDomain;
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

            if (repoType.Equals(RepositoryTypeConstants.Database, StringComparison.OrdinalIgnoreCase))
                return new QuantityMeasurementDatabaseRepository(DatabaseConfig.GetConnectionString());

            return QuantityMeasurementCacheRepository.Instance;
        }

        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            Logger.Info("Comparing quantities");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            bool result = a.Equals(b);

            var entity = QuantityMeasurementEntityFactory.CreateBinaryOperation(
                q1,
                q2,
                OperationTypeConstants.Compare,
                result.ToString(),
                string.Empty);

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

            var entity = QuantityMeasurementEntityFactory.CreateBinaryOperation(
                q1,
                q2,
                OperationTypeConstants.Add,
                result.Value.ToString(),
                targetUnit);

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

            var entity = QuantityMeasurementEntityFactory.CreateBinaryOperation(
                q1,
                q2,
                OperationTypeConstants.Subtract,
                result.Value.ToString(),
                targetUnit);

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

            var entity = QuantityMeasurementEntityFactory.CreateBinaryOperation(
                q1,
                q2,
                OperationTypeConstants.Divide,
                result.ToString(),
                string.Empty);

            repository.Save(entity);

            Logger.Debug($"Divide result: {result}");

            return result;
        }

        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            Logger.Info("Performing CONVERT operation");

            dynamic q = CreateQuantity(quantity);

            var result = ConvertToTarget(q, targetUnit, quantity.MeasurementType);

            var entity = QuantityMeasurementEntityFactory.CreateConversionOperation(
                quantity,
                targetUnit,
                result.Value.ToString());

            repository.Save(entity);

            Logger.Debug($"Convert result: {result.Value}");

            return CreateDTO(result, quantity.MeasurementType);
        }

        // ---------------- HELPERS ----------------

        private static void ValidateType(QuantityDTO q1, QuantityDTO q2)
        {
            if (!string.Equals(q1.MeasurementType, q2.MeasurementType, StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException("Different measurement types cannot be compared");
        }

        private static dynamic ParseTargetUnit(string unit, string type)
        {
            return MeasurementTypeDispatcher.ParseTargetUnit(unit, type);
        }

        private static dynamic CreateQuantity(QuantityDTO dto)
        {
            return MeasurementTypeDispatcher.CreateQuantity(dto);
        }

        private static dynamic ConvertToTarget(dynamic quantity, string targetUnit, string type)
        {
            return MeasurementTypeDispatcher.ConvertToTarget(quantity, targetUnit, type);
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