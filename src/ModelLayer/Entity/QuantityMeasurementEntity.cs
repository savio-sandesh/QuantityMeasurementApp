using System;

namespace ModelLayer
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public double FirstValue { get; set; }
        public string FirstUnit { get; set; }
        public string FirstMeasurementType { get; set; }

        public double SecondValue { get; set; }
        public string SecondUnit { get; set; }
        public string SecondMeasurementType { get; set; }

        public string Operation { get; set; }

        public string ResultValue { get; set; }
        public string ResultUnit { get; set; }
        public string ResultMeasurementType { get; set; }

        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

        public QuantityMeasurementEntity() { }

        public QuantityMeasurementEntity(
            double firstValue,
            string firstUnit,
            double secondValue,
            string secondUnit,
            string operation,
            string resultValue)
        {
            FirstValue = firstValue;
            FirstUnit = firstUnit;

            SecondValue = secondValue;
            SecondUnit = secondUnit;

            Operation = operation;
            ResultValue = resultValue;
        }
    }
}