namespace ModelLayer
{
    public class QuantityMeasurementDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public double ThisValue { get; set; }
        public string ThisUnit { get; set; } = string.Empty;
        public string ThisMeasurementType { get; set; } = string.Empty;

        public double ThatValue { get; set; }
        public string ThatUnit { get; set; } = string.Empty;
        public string ThatMeasurementType { get; set; } = string.Empty;

        public string ResultValue { get; set; } = string.Empty;
        public string ResultUnit { get; set; } = string.Empty;
        public string ResultString { get; set; } = string.Empty;

        public string Operation { get; set; } = string.Empty;
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
