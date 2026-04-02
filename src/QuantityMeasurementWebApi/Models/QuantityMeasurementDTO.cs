using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementWebApi.Models
{
    public class QuantityMeasurementDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public double ThisValue { get; set; }

        [Required]
        public string ThisUnit { get; set; } = string.Empty;

        [Required]
        public string ThisMeasurementType { get; set; } = string.Empty;

        public double ThatValue { get; set; }

        [Required]
        public string ThatUnit { get; set; } = string.Empty;

        [Required]
        public string ThatMeasurementType { get; set; } = string.Empty;

        public decimal ResultValue { get; set; }

        [Required]
        public string ResultUnit { get; set; } = string.Empty;

        [Required]
        public string TargetUnit { get; set; } = string.Empty;

        [Required]
        public string ResultString { get; set; } = string.Empty;

        [Required]
        public string Operation { get; set; } = string.Empty;

        public bool IsError { get; set; }

        [Required]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
