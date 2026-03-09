namespace QuantityMeasurementApp
{
    /// <summary>
    /// Enumeration representing the units of length measurement.
    /// </summary>
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yard,
        Centimeter
    }

    /// <summary>
    /// Extension methods for LengthUnit conversion behavior.
    /// </summary>
    public static class LengthUnitExtensions
    {
        /// <summary>
        /// Returns the conversion factor relative to feet.
        /// </summary>
        public static double ToFeetFactor(this LengthUnit unit)
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
        /// Validates that the enum value is a defined LengthUnit constant.
        /// </summary>
        public static bool IsDefined(this LengthUnit unit)
        {
            return Enum.IsDefined(typeof(LengthUnit), unit);
        }
    }
}
