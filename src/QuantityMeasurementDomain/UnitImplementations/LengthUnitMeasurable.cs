using QuantityMeasurementDomain.Interface;

namespace QuantityMeasurementDomain
{
    /// <summary>
    /// Measurable adapter for length units.
    /// </summary>
    public sealed class LengthUnitMeasurable : IMeasurableUnit<LengthUnit>
    {
        public static readonly LengthUnitMeasurable Instance = new();

        private LengthUnitMeasurable()
        {
        }

        public double GetConversionFactor(LengthUnit unit)
        {
            return unit.GetConversionFactor();
        }

        public double ConvertToBaseUnit(LengthUnit unit, double value)
        {
            return unit.ConvertToBaseUnit(value);
        }

        public double ConvertFromBaseUnit(LengthUnit unit, double baseValue)
        {
            return unit.ConvertFromBaseUnit(baseValue);
        }

        public string GetUnitName(LengthUnit unit)
        {
            return unit.ToString();
        }
    }
}
