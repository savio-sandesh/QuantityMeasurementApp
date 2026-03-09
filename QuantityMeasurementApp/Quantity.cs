namespace QuantityMeasurementApp
{
    /// <summary>
    /// Generic immutable quantity that supports conversion, equality, and arithmetic
    /// for any registered measurable enum category.
    /// </summary>
    public class Quantity<TUnit> where TUnit : struct, Enum
    {
        private const double Tolerance = 0.000001;
        private readonly IMeasurableUnit<TUnit> measurable;
        private readonly double valueInBaseUnit;

        public double Value { get; }
        public TUnit Unit { get; }

        public Quantity(double value, TUnit unit)
        {
            ValidateFinite(value);
            ValidateUnit(unit, nameof(unit));

            measurable = MeasurableRegistry.For<TUnit>();
            Value = value;
            Unit = unit;
            valueInBaseUnit = measurable.ConvertToBaseUnit(unit, value);
        }

        public Quantity<TUnit> ConvertTo(TUnit targetUnit)
        {
            ValidateUnit(targetUnit, nameof(targetUnit));

            double converted = measurable.ConvertFromBaseUnit(targetUnit, valueInBaseUnit);
            return new Quantity<TUnit>(converted, targetUnit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other)
        {
            return Add(other, Unit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other, TUnit targetUnit)
        {
            return PerformQuantityArithmetic(other, targetUnit, ArithmeticOperation.Add);
        }

        public static Quantity<TUnit> Add(Quantity<TUnit> first, Quantity<TUnit> second)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.Add(second);
        }

        public static Quantity<TUnit> Add(Quantity<TUnit> first, Quantity<TUnit> second, TUnit targetUnit)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.Add(second, targetUnit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other)
        {
            return Subtract(other, Unit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other, TUnit targetUnit)
        {
            return PerformQuantityArithmetic(other, targetUnit, ArithmeticOperation.Subtract);
        }

        public static Quantity<TUnit> Subtract(Quantity<TUnit> first, Quantity<TUnit> second)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.Subtract(second);
        }

        public static Quantity<TUnit> Subtract(Quantity<TUnit> first, Quantity<TUnit> second, TUnit targetUnit)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.Subtract(second, targetUnit);
        }

        public double Divide(Quantity<TUnit> other)
        {
            return PerformScalarArithmetic(other, ArithmeticOperation.Divide);
        }

        public static double Divide(Quantity<TUnit> first, Quantity<TUnit> second)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.Divide(second);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not Quantity<TUnit> other)
            {
                return false;
            }

            return Math.Abs(valueInBaseUnit - other.valueInBaseUnit) <= Tolerance;
        }

        public override int GetHashCode()
        {
            double normalized = Math.Round(valueInBaseUnit / Tolerance) * Tolerance;
            return HashCode.Combine(typeof(TUnit), normalized);
        }

        public override string ToString()
        {
            return $"Quantity({Value:0.######}, {measurable.GetUnitName(Unit)})";
        }

        private static void ValidateFinite(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Value must be a finite number", nameof(value));
            }
        }

        private static void ValidateUnit(TUnit unit, string parameterName)
        {
            if (!Enum.IsDefined(typeof(TUnit), unit))
            {
                throw new ArgumentException("Unsupported unit", parameterName);
            }
        }

        // Centralized path for all quantity-returning arithmetic operations.
        private Quantity<TUnit> PerformQuantityArithmetic(Quantity<TUnit> other, TUnit targetUnit, ArithmeticOperation operation)
        {
            ValidateArithmeticOperands(other, targetUnit, targetUnitRequired: true);

            double resultInBase = Compute(operation, valueInBaseUnit, other.valueInBaseUnit);
            double resultValue = measurable.ConvertFromBaseUnit(targetUnit, resultInBase);

            return new Quantity<TUnit>(resultValue, targetUnit);
        }

        private double PerformScalarArithmetic(Quantity<TUnit> other, ArithmeticOperation operation)
        {
            ValidateArithmeticOperands(other, targetUnit: null, targetUnitRequired: false);
            return Compute(operation, valueInBaseUnit, other.valueInBaseUnit);
        }

        private static void ValidateArithmeticOperands(Quantity<TUnit> other, TUnit? targetUnit, bool targetUnitRequired)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            ValidateFinite(other.Value);
            ValidateUnit(other.Unit, nameof(other));

            if (targetUnitRequired)
            {
                if (!targetUnit.HasValue)
                {
                    throw new ArgumentNullException(nameof(targetUnit));
                }

                ValidateUnit(targetUnit.Value, nameof(targetUnit));
            }
        }

        private enum ArithmeticOperation
        {
            Add,
            Subtract,
            Divide
        }

        private static double Compute(ArithmeticOperation operation, double left, double right)
        {
            return operation switch
            {
                ArithmeticOperation.Add => left + right,
                ArithmeticOperation.Subtract => left - right,
                ArithmeticOperation.Divide => ComputeDivision(left, right),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, "Unsupported arithmetic operation")
            };
        }

        private static double ComputeDivision(double left, double right)
        {
            if (Math.Abs(right) <= Tolerance)
            {
                throw new ArithmeticException("Division by zero quantity is not allowed.");
            }

            return left / right;
        }
    }
}
