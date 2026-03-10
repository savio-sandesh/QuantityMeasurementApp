namespace QuantityMeasurementApp
{
    /// <summary>
    /// C# adaptation of UC10 measurable contract for enum-based units.
    /// </summary>
    public interface IMeasurableUnit<TUnit> where TUnit : struct, Enum
    {
        double GetConversionFactor(TUnit unit);
        double ConvertToBaseUnit(TUnit unit, double value);
        double ConvertFromBaseUnit(TUnit unit, double baseValue);
        string GetUnitName(TUnit unit);

        // Lambda-style capability hook. By default, all measurable categories support arithmetic.
        Func<bool> SupportsArithmetic => () => true;

        bool SupportsArithmeticOperations()
        {
            return SupportsArithmetic();
        }

        void ValidateOperationSupport(string operation)
        {
            // Default no-op keeps existing measurable categories backward compatible.
        }
    }
}
