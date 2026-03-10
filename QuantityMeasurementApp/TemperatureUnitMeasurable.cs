namespace QuantityMeasurementApp
{
    /// <summary>
    /// Measurable adapter for temperature units with non-linear conversion formulas.
    /// Arithmetic on absolute temperatures is blocked in UC14.
    /// </summary>
    public sealed class TemperatureUnitMeasurable : IMeasurableUnit<TemperatureUnit>
    {
        public static readonly TemperatureUnitMeasurable Instance = new();

        private TemperatureUnitMeasurable()
        {
        }

        public Func<bool> SupportsArithmetic => () => false;

        public double GetConversionFactor(TemperatureUnit unit)
        {
            // Temperature conversion is non-linear, so a single multiplicative factor is not meaningful.
            return 1.0;
        }

        public double ConvertToBaseUnit(TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => value,
                TemperatureUnit.Fahrenheit => (value - 32.0) * (5.0 / 9.0),
                TemperatureUnit.Kelvin => value - 273.15,
                _ => throw new ArgumentException("Unsupported temperature unit", nameof(unit))
            };
        }

        public double ConvertFromBaseUnit(TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => baseValue,
                TemperatureUnit.Fahrenheit => (baseValue * (9.0 / 5.0)) + 32.0,
                TemperatureUnit.Kelvin => baseValue + 273.15,
                _ => throw new ArgumentException("Unsupported temperature unit", nameof(unit))
            };
        }

        public string GetUnitName(TemperatureUnit unit)
        {
            return unit.ToString();
        }

        public void ValidateOperationSupport(string operation)
        {
            if (((IMeasurableUnit<TemperatureUnit>)this).SupportsArithmeticOperations())
            {
                return;
            }

            throw new UnsupportedOperationException($"Temperature does not support '{operation}' operation for absolute values.");
        }
    }
}
