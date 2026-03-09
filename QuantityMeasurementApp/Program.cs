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
            Console.WriteLine("=== Quantity Measurement App ===");
            Console.WriteLine("1. Compare two lengths");
            Console.WriteLine("2. Convert units");

            int option = ReadOption("Choose an option (1 or 2): ");

            if (option == 1)
            {
                DemonstrateLengthComparison();
                return;
            }

            DemonstrateLengthConversion();
        }

        private static void DemonstrateLengthComparison()
        {
            Console.WriteLine("=== Length Equality ===");

            double firstValue = ReadDouble("Enter first numeric value: ");
            LengthUnit firstUnit = ReadUnit("Enter first unit (Feet, Inch, Yard, Centimeter): ");

            double secondValue = ReadDouble("Enter second numeric value: ");
            LengthUnit secondUnit = ReadUnit("Enter second unit (Feet, Inch, Yard, Centimeter): ");

            Length firstMeasurement = new Length(firstValue, firstUnit);
            Length secondMeasurement = new Length(secondValue, secondUnit);

            bool result = firstMeasurement.Equals(secondMeasurement);

            Console.WriteLine($"Equality Result: {result}");
        }

        private static void DemonstrateLengthConversion()
        {
            Console.WriteLine("=== Unit Conversion ===");

            double value = ReadDouble("Enter value to convert: ");
            LengthUnit sourceUnit = ReadUnit("Enter source unit (Feet, Inch, Yard, Centimeter): ");
            LengthUnit targetUnit = ReadUnit("Enter target unit (Feet, Inch, Yard, Centimeter): ");

            double converted = Length.Convert(value, sourceUnit, targetUnit);
            Console.WriteLine($"Converted Value: {converted}");
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

        private static int ReadOption(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int option) && (option == 1 || option == 2))
                    return option;

                Console.WriteLine("Invalid option. Enter 1 or 2.");
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