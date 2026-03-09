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
            Console.WriteLine("2. Convert length units");
            Console.WriteLine("3. Add two lengths");
            Console.WriteLine("4. Compare two weights");
            Console.WriteLine("5. Convert weight units");
            Console.WriteLine("6. Add two weights");

            int option = ReadOption("Choose an option (1-6): ");

            if (option == 1)
            {
                DemonstrateLengthComparison();
                return;
            }

            if (option == 2)
            {
                DemonstrateLengthConversion();
                return;
            }

            if (option == 3)
            {
                DemonstrateLengthAddition();
                return;
            }

            if (option == 4)
            {
                DemonstrateWeightComparison();
                return;
            }

            if (option == 5)
            {
                DemonstrateWeightConversion();
                return;
            }

            DemonstrateWeightAddition();
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

        private static void DemonstrateLengthAddition()
        {
            Console.WriteLine("=== Length Addition (Target Unit) ===");

            double firstValue = ReadDouble("Enter first numeric value: ");
            LengthUnit firstUnit = ReadUnit("Enter first unit (Feet, Inch, Yard, Centimeter): ");

            double secondValue = ReadDouble("Enter second numeric value: ");
            LengthUnit secondUnit = ReadUnit("Enter second unit (Feet, Inch, Yard, Centimeter): ");
            LengthUnit targetUnit = ReadUnit("Enter target result unit (Feet, Inch, Yard, Centimeter): ");

            var first = new Length(firstValue, firstUnit);
            var second = new Length(secondValue, secondUnit);
            var sum = first.Add(second, targetUnit);

            Console.WriteLine($"Sum Value: {sum.ConvertTo(targetUnit)} {targetUnit}");
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

                if (int.TryParse(input, out int option) && option >= 1 && option <= 6)
                    return option;

                Console.WriteLine("Invalid option. Enter a value from 1 to 6.");
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

        private static void DemonstrateWeightComparison()
        {
            Console.WriteLine("=== Weight Equality ===");

            double firstValue = ReadDouble("Enter first numeric value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first unit (Kilogram, Gram, Pound): ");

            double secondValue = ReadDouble("Enter second numeric value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second unit (Kilogram, Gram, Pound): ");

            var firstMeasurement = new Weight(firstValue, firstUnit);
            var secondMeasurement = new Weight(secondValue, secondUnit);

            bool result = firstMeasurement.Equals(secondMeasurement);

            Console.WriteLine($"Equality Result: {result}");
        }

        private static void DemonstrateWeightConversion()
        {
            Console.WriteLine("=== Weight Unit Conversion ===");

            double value = ReadDouble("Enter value to convert: ");
            WeightUnit sourceUnit = ReadWeightUnit("Enter source unit (Kilogram, Gram, Pound): ");
            WeightUnit targetUnit = ReadWeightUnit("Enter target unit (Kilogram, Gram, Pound): ");

            double converted = Weight.Convert(value, sourceUnit, targetUnit);
            Console.WriteLine($"Converted Value: {converted}");
        }

        private static void DemonstrateWeightAddition()
        {
            Console.WriteLine("=== Weight Addition (Target Unit) ===");

            double firstValue = ReadDouble("Enter first numeric value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first unit (Kilogram, Gram, Pound): ");

            double secondValue = ReadDouble("Enter second numeric value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second unit (Kilogram, Gram, Pound): ");
            WeightUnit targetUnit = ReadWeightUnit("Enter target result unit (Kilogram, Gram, Pound): ");

            var first = new Weight(firstValue, firstUnit);
            var second = new Weight(secondValue, secondUnit);
            var sum = first.Add(second, targetUnit);

            Console.WriteLine($"Sum Value: {sum.ConvertTo(targetUnit)} {targetUnit}");
        }

        private static WeightUnit ReadWeightUnit(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (Enum.TryParse<WeightUnit>(input, ignoreCase: true, out var unit))
                    return unit;

                Console.WriteLine("Invalid unit. Use one of: Kilogram, Gram, Pound.");
            }
        }
    }
}