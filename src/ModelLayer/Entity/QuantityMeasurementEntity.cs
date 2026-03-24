using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer
{
    [Serializable]
    [Table("Measurements")]
    public class QuantityMeasurementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Value1")]
        public double Value1 { get; set; }

        [Column("Unit1")]
        public string Unit1 { get; set; } = string.Empty;

        [Column("Value2")]
        public double Value2 { get; set; }

        [Column("Unit2")]
        public string Unit2 { get; set; } = string.Empty;

        [Column("Result")]
        public string Result { get; set; } = string.Empty;

        [Column("OperationType")]
        public string OperationType { get; set; } = string.Empty;

        [Column("MeasurementType")]
        public string MeasurementType { get; set; } = string.Empty;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        // Backward-compatible aliases used by existing business/repository code.
        [NotMapped]
        public double FirstValue { get => Value1; set => Value1 = value; }

        [NotMapped]
        public string FirstUnit { get => Unit1; set => Unit1 = value; }

        [NotMapped]
        public string FirstMeasurementType { get => MeasurementType; set => MeasurementType = value; }

        [NotMapped]
        public double SecondValue { get => Value2; set => Value2 = value; }

        [NotMapped]
        public string SecondUnit { get => Unit2; set => Unit2 = value; }

        [NotMapped]
        public string SecondMeasurementType { get => MeasurementType; set => MeasurementType = value; }

        [NotMapped]
        public string Operation { get => OperationType; set => OperationType = value; }

        [NotMapped]
        public string ResultValue { get => Result; set => Result = value; }

        [NotMapped]
        public string ResultUnit { get; set; } = string.Empty;

        [NotMapped]
        public string ResultMeasurementType { get => MeasurementType; set => MeasurementType = value; }

        [NotMapped]
        public bool IsError { get; set; }

        [NotMapped]
        public string ErrorMessage { get; set; } = string.Empty;

        public QuantityMeasurementEntity()
        {
            CreatedAt = DateTime.Now;
        }

        public QuantityMeasurementEntity(
            double firstValue,
            string firstUnit,
            double secondValue,
            string secondUnit,
            string operation,
            string resultValue)
        {
            Value1 = firstValue;
            Unit1 = firstUnit;
            Value2 = secondValue;
            Unit2 = secondUnit;
            OperationType = operation;
            Result = resultValue;
            CreatedAt = DateTime.Now;
        }
    }
}