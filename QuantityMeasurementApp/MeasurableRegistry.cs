namespace QuantityMeasurementApp
{
    /// <summary>
    /// Resolves measurable adapters for supported unit categories.
    /// </summary>
    public static class MeasurableRegistry
    {
        public static IMeasurableUnit<TUnit> For<TUnit>() where TUnit : struct, Enum
        {
            if (typeof(TUnit) == typeof(LengthUnit))
            {
                return (IMeasurableUnit<TUnit>)(object)LengthUnitMeasurable.Instance;
            }

            if (typeof(TUnit) == typeof(WeightUnit))
            {
                return (IMeasurableUnit<TUnit>)(object)WeightUnitMeasurable.Instance;
            }

            if (typeof(TUnit) == typeof(VolumeUnit))
            {
                return (IMeasurableUnit<TUnit>)(object)VolumeUnitMeasurable.Instance;
            }

            throw new ArgumentException($"No measurable adapter registered for unit type '{typeof(TUnit).Name}'.", nameof(TUnit));
        }
    }
}
