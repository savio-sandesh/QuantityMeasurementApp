namespace QuantityMeasurementApp
{
    /// <summary>
    /// Enumeration representing supported length units.
    /// Each enum value stores its conversion factor relative to feet (base unit).
    /// </summary>
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yard,
        Centimeter
    }

    /// <summary>
    /// Conversion behavior for LengthUnit values.
    /// </summary>
    public static class LengthUnitConversions
    {

        /// <summary>
        /// Returns this unit's factor relative to feet.
        /// </summary>
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => 1.0,
                LengthUnit.Inch => 1.0 / 12.0,
                LengthUnit.Yard => 3.0,
                LengthUnit.Centimeter => 0.393701 / 12.0,
                _ => throw new ArgumentException("Unsupported length unit", nameof(unit))
            };
        }

        /// <summary>
        /// Converts a value in this unit to feet (base unit).
        /// </summary>
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts a value from feet (base unit) to this unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }
    }
}
