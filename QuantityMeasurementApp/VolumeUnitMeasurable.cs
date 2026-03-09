namespace QuantityMeasurementApp
{
    /// <summary>
    /// Measurable adapter for volume units.
    /// </summary>
    public sealed class VolumeUnitMeasurable : IMeasurableUnit<VolumeUnit>
    {
        public static readonly VolumeUnitMeasurable Instance = new();

        private VolumeUnitMeasurable()
        {
        }

        public double GetConversionFactor(VolumeUnit unit)
        {
            return unit.GetConversionFactor();
        }

        public double ConvertToBaseUnit(VolumeUnit unit, double value)
        {
            return unit.ConvertToBaseUnit(value);
        }

        public double ConvertFromBaseUnit(VolumeUnit unit, double baseValue)
        {
            return unit.ConvertFromBaseUnit(baseValue);
        }

        public string GetUnitName(VolumeUnit unit)
        {
            return unit.ToString();
        }
    }
}
