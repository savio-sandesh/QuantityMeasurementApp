namespace QuantityMeasurementApp
{
    /// <summary>
    /// Measurable adapter for weight units.
    /// </summary>
    public sealed class WeightUnitMeasurable : IMeasurableUnit<WeightUnit>
    {
        public static readonly WeightUnitMeasurable Instance = new();

        private WeightUnitMeasurable()
        {
        }

        public double GetConversionFactor(WeightUnit unit)
        {
            return unit.GetConversionFactor();
        }

        public double ConvertToBaseUnit(WeightUnit unit, double value)
        {
            return unit.ConvertToBaseUnit(value);
        }

        public double ConvertFromBaseUnit(WeightUnit unit, double baseValue)
        {
            return unit.ConvertFromBaseUnit(baseValue);
        }

        public string GetUnitName(WeightUnit unit)
        {
            return unit.ToString();
        }
    }
}
