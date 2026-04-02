namespace QuantityMeasurementWebApi.Models
{
    public class MathRequestDTO
    {
        public MathQuantityDTO? FirstQuantityDTO { get; set; }

        public MathQuantityDTO? SecondQuantityDTO { get; set; }

        public string? TargetUnit { get; set; }

        public string? MeasurementType { get; set; }
    }

    public class MathQuantityDTO
    {
        public double Value { get; set; }

        public string? Unit { get; set; }
    }
}
