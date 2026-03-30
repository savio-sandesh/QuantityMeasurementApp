using System;
using System.Collections.Generic;
using System.Linq;
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

        public QuantityMeasurementDTO Compare(QuantityDTO q1, QuantityDTO q2, int userId)
        {
            Logger.Info("Comparing quantities");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            bool result = a.Equals(b);
            string resultText = result.ToString();

            var entity = QuantityMeasurementEntityFactory.CreateBinaryOperation(
                q1,
                q2,
                OperationTypeConstants.Compare,
                resultText,
                string.Empty,
                userId);

            repository.Save(entity);

            Logger.Debug($"Compare result: {result}");

            return MapToQuantityMeasurementDto(entity, resultText, string.Empty, resultText);
        }

        public QuantityMeasurementDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit, int userId)
        {
            Logger.Info("Performing ADD operation");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            var result = a.Add(b, ParseTargetUnit(targetUnit, q1.MeasurementType));
            string resultValue = result.Value.ToString();

            var entity = QuantityMeasurementEntityFactory.CreateBinaryOperation(
                q1,
                q2,
                OperationTypeConstants.Add,
                resultValue,
                targetUnit,
                userId);

            repository.Save(entity);

            Logger.Debug($"Add result: {result.Value}");

            return MapToQuantityMeasurementDto(entity, resultValue, targetUnit, resultValue);
        }

        public QuantityMeasurementDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            Logger.Info("Performing SUBTRACT operation");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            var result = a.Subtract(b, ParseTargetUnit(targetUnit, q1.MeasurementType));
            string resultValue = result.Value.ToString();

            var entity = QuantityMeasurementEntityFactory.CreateBinaryOperation(
                q1,
                q2,
                OperationTypeConstants.Subtract,
                resultValue,
                targetUnit,
                0);

            repository.Save(entity);

            Logger.Debug($"Subtract result: {result.Value}");

            return MapToQuantityMeasurementDto(entity, resultValue, targetUnit, resultValue);
        }

        public QuantityMeasurementDTO Divide(QuantityDTO q1, QuantityDTO q2)
        {
            Logger.Info("Performing DIVIDE operation");

            ValidateType(q1, q2);

            dynamic a = CreateQuantity(q1);
            dynamic b = CreateQuantity(q2);

            double result = a.Divide(b);
            string resultText = result.ToString();

            var entity = QuantityMeasurementEntityFactory.CreateBinaryOperation(
                q1,
                q2,
                OperationTypeConstants.Divide,
                resultText,
                string.Empty,
                0);

            repository.Save(entity);

            Logger.Debug($"Divide result: {result}");

            return MapToQuantityMeasurementDto(entity, resultText, string.Empty, resultText);
        }

        public QuantityMeasurementDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            Logger.Info("Performing CONVERT operation");

            dynamic q = CreateQuantity(quantity);

            var result = ConvertToTarget(q, targetUnit, quantity.MeasurementType);
            string resultValue = result.Value.ToString();

            var entity = QuantityMeasurementEntityFactory.CreateConversionOperation(
                quantity,
                targetUnit,
                resultValue);

            repository.Save(entity);

            Logger.Debug($"Convert result: {result.Value}");

            return MapToQuantityMeasurementDto(entity, resultValue, targetUnit, resultValue);
        }

        public List<QuantityMeasurementDTO> GetOperationHistory(string operation)
        {
            var entities = repository.GetByOperation(operation);
            return entities.Select(MapFromEntity).ToList();
        }

        public List<QuantityMeasurementDTO> GetMeasurementsByType(string type)
        {
            var entities = repository.GetByType(type);
            return entities.Select(MapFromEntity).ToList();
        }

        public List<QuantityMeasurementDTO> GetHistoryByUserId(int userId)
        {
            var entities = repository.GetHistoryByUserId(userId);
            return entities.Select(MapFromEntity).ToList();
        }

        public int GetOperationCount(string operation)
        {
            return repository.GetByOperation(operation).Count;
        }

        public List<QuantityMeasurementDTO> GetErroredOperations()
        {
            var entities = repository.GetAllMeasurements();
            return entities
                .Where(x => x.IsError)
                .Select(MapFromEntity)
                .ToList();
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

        private static QuantityMeasurementDTO MapToQuantityMeasurementDto(
            QuantityMeasurementEntity entity,
            string resultValue,
            string resultUnit,
            string resultString)
        {
            return new QuantityMeasurementDTO
            {
                ThisValue = entity.FirstValue,
                ThisUnit = entity.FirstUnit,
                ThisMeasurementType = entity.FirstMeasurementType,
                ThatValue = entity.SecondValue,
                ThatUnit = entity.SecondUnit,
                ThatMeasurementType = entity.SecondMeasurementType,
                ResultValue = resultValue,
                ResultUnit = resultUnit,
                ResultString = resultString,
                Operation = entity.Operation,
                IsError = entity.IsError,
                ErrorMessage = entity.ErrorMessage
            };
        }

        private static QuantityMeasurementDTO MapFromEntity(QuantityMeasurementEntity entity)
        {
            return new QuantityMeasurementDTO
            {
                ThisValue = entity.FirstValue,
                ThisUnit = entity.FirstUnit,
                ThisMeasurementType = entity.FirstMeasurementType,
                ThatValue = entity.SecondValue,
                ThatUnit = entity.SecondUnit,
                ThatMeasurementType = entity.SecondMeasurementType,
                ResultValue = entity.ResultValue,
                ResultUnit = entity.ResultUnit,
                ResultString = entity.ResultValue,
                Operation = entity.Operation,
                IsError = entity.IsError,
                ErrorMessage = entity.ErrorMessage
            };
        }
    }
}