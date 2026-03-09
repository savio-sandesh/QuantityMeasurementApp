namespace QuantityMeasurementApp
{
    /// <summary>
    /// Enumeration representing supported weight units.
    /// Each enum value stores its conversion factor relative to kilograms (base unit).
    /// </summary>
    public enum WeightUnit
    {
        Kilogram,
        Gram,
        Pound
    }

    /// <summary>
    /// Conversion behavior for WeightUnit values.
    /// </summary>
    public static class WeightUnitConversions
    {
        /// <summary>
        /// Returns this unit's factor relative to kilograms.
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Kilogram => 1.0,
                WeightUnit.Gram => 0.001,
                WeightUnit.Pound => 0.453592,
                _ => throw new ArgumentException("Unsupported weight unit", nameof(unit))
            };
        }

        /// <summary>
        /// Converts a value in this unit to kilograms (base unit).
        /// </summary>
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts a value from kilograms (base unit) to this unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }
    }
}
