using ModelLayer;

namespace BusinessLayer
{
    internal static class QuantityMeasurementEntityFactory
    {
        public static QuantityMeasurementEntity CreateBinaryOperation(
            QuantityDTO first,
            QuantityDTO second,
            string operation,
            string resultValue,
            string resultUnit,
            int userId)
        {
            return new QuantityMeasurementEntity
            {
                FirstValue = first.Value,
                FirstUnit = first.Unit,
                FirstMeasurementType = first.MeasurementType,
                SecondValue = second.Value,
                SecondUnit = second.Unit,
                SecondMeasurementType = second.MeasurementType,
                ResultValue = resultValue,
                ResultUnit = resultUnit,
                ResultMeasurementType = first.MeasurementType,
                Operation = operation,
                UserId = userId,
                IsError = false,
                ErrorMessage = string.Empty
            };
        }

        public static QuantityMeasurementEntity CreateConversionOperation(
            QuantityDTO source,
            string targetUnit,
            string resultValue,
            int userId)
        {
            return new QuantityMeasurementEntity
            {
                FirstValue = source.Value,
                FirstUnit = source.Unit,
                FirstMeasurementType = source.MeasurementType,
                SecondValue = 0,
                SecondUnit = targetUnit,
                SecondMeasurementType = source.MeasurementType,
                ResultValue = resultValue,
                ResultUnit = targetUnit,
                ResultMeasurementType = source.MeasurementType,
                Operation = OperationTypeConstants.Convert,
                UserId = userId,
                IsError = false,
                ErrorMessage = string.Empty
            };
        }
    }
}