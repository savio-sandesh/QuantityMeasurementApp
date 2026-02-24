using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Entry point for UC1: Feet Measurement Equality.
    /// Handles user interaction and delegates equality comparison
    /// to the LengthInFeet value object.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== UC1: Feet Measurement Equality ===");

            double firstValue = ReadDouble("Enter first value in feet: ");
            double secondValue = ReadDouble("Enter second value in feet: ");

            LengthInFeet firstMeasurement = new LengthInFeet(firstValue);
            LengthInFeet secondMeasurement = new LengthInFeet(secondValue);

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
    }
}