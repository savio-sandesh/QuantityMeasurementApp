using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Entry point of Quantity Measurement Application.
    /// Responsible for user interaction and calling domain logic.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== UC1: Feet Measurement Equality ===");

            double firstValue = ReadDouble("Enter first value in feet: ");
            double secondValue = ReadDouble("Enter second value in feet: ");

            var firstMeasurement = new QuantityMeasurement(firstValue);
            var secondMeasurement = new QuantityMeasurement(secondValue);

            bool result = firstMeasurement.Equals(secondMeasurement);

            Console.WriteLine($"Equality Result: {result}");
        }

        /// <summary>
        /// Reads and validates numeric input from console.
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