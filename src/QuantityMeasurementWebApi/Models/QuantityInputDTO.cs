using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementWebApi.Models
{
    public class QuantityInputDTO
    {
        [Required]
        public QuantityDTO ThisQuantityDTO { get; set; } = null!;

        [Required]
        public QuantityDTO ThatQuantityDTO { get; set; } = null!;

        public QuantityDTO? TargetQuantityDTO { get; set; }
    }
}
