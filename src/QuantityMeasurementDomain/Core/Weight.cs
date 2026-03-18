namespace QuantityMeasurementDomain
{
    /// <summary>
    /// Represents an immutable weight measurement.
    /// </summary>
    public class Weight
    {
        private readonly double valueInKilograms;
        private const double Tolerance = 0.000001;

        public WeightUnit Unit { get; }

        public Weight(double value, WeightUnit unit)
        {
            ValidateFinite(value);
            ValidateUnit(unit, nameof(unit));

            Unit = unit;
            valueInKilograms = unit.ConvertToBaseUnit(value);
        }

        public static double Convert(double value, WeightUnit sourceUnit, WeightUnit targetUnit)
        {
            ValidateFinite(value);
            ValidateUnit(sourceUnit, nameof(sourceUnit));
            ValidateUnit(targetUnit, nameof(targetUnit));

            double valueInKg = sourceUnit.ConvertToBaseUnit(value);
            return targetUnit.ConvertFromBaseUnit(valueInKg);
        }

        public double ConvertTo(WeightUnit targetUnit)
        {
            ValidateUnit(targetUnit, nameof(targetUnit));
            return targetUnit.ConvertFromBaseUnit(valueInKilograms);
        }

        public Weight Add(Weight other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Add(this, other, Unit);
        }

        public Weight Add(Weight other, WeightUnit targetUnit)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Add(this, other, targetUnit);
        }

        public static Weight Add(Weight first, Weight second, WeightUnit targetUnit)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            ValidateUnit(targetUnit, nameof(targetUnit));

            double sumInKg = first.valueInKilograms + second.valueInKilograms;
            double resultValue = targetUnit.ConvertFromBaseUnit(sumInKg);

            return new Weight(resultValue, targetUnit);
        }

        public static Weight Add(Weight first, Weight second)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return Add(first, second, first.Unit);
        }

        public static Weight Add(double firstValue, WeightUnit firstUnit, double secondValue, WeightUnit secondUnit, WeightUnit targetUnit)
        {
            var first = new Weight(firstValue, firstUnit);
            var second = new Weight(secondValue, secondUnit);

            return Add(first, second, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not Weight other)
            {
                return false;
            }

            return Math.Abs(valueInKilograms - other.valueInKilograms) <= Tolerance;
        }

        public override int GetHashCode()
        {
            double normalized = Math.Round(valueInKilograms / Tolerance) * Tolerance;
            return normalized.GetHashCode();
        }

        public override string ToString()
        {
            return $"{ConvertTo(Unit):0.######} {Unit}";
        }

        private static void ValidateFinite(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Value must be a finite number", nameof(value));
            }
        }

        private static void ValidateUnit(WeightUnit unit, string parameterName)
        {
            if (!Enum.IsDefined(typeof(WeightUnit), unit))
            {
                throw new ArgumentException("Unsupported weight unit", parameterName);
            }
        }
    }
}