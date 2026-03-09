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
    }
}
