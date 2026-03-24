using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementWebApi.Models
{
    public class QuantityDTO
    {
        [Required]
        public double Value { get; set; }

        [Required]
        public string Unit { get; set; } = string.Empty;

        [Required]
        public string MeasurementType { get; set; } = string.Empty;
    }
}
