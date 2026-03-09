using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Entry point for generic length equality comparison.
    /// Supports Feet, Inch, Yard, and Centimeter units.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Quantity Length Equality ===");

            double firstValue = ReadDouble("Enter first numeric value: ");
            LengthUnit firstUnit = ReadUnit("Enter first unit (Feet, Inch, Yard, Centimeter): ");

            double secondValue = ReadDouble("Enter second numeric value: ");
            LengthUnit secondUnit = ReadUnit("Enter second unit (Feet, Inch, Yard, Centimeter): ");

            Length firstMeasurement = new Length(firstValue, firstUnit);
            Length secondMeasurement = new Length(secondValue, secondUnit);

            bool result = firstMeasurement.Equals(secondMeasurement);

            Console.WriteLine($"Equality Result: {result}");
        }

        /// <summary>
        /// Reads numeric input from console with validation.
        /// </summary>
        private static double ReadDouble(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (double.TryParse(input, out double parsedValue))
                    return parsedValue;

                Console.WriteLine("Invalid number format. Please enter a valid numeric value.");
            }
        }

        /// <summary>
        /// Reads unit input from console with validation.
        /// </summary>
        private static LengthUnit ReadUnit(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (Enum.TryParse<LengthUnit>(input, ignoreCase: true, out var unit))
                    return unit;

                Console.WriteLine("Invalid unit. Use one of: Feet, Inch, Yard, Centimeter.");
            }
        }
    }
}