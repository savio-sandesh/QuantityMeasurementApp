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
            Console.WriteLine("7. Run generic quantity demo (UC10-UC14)");

            int option = ReadOption("Choose an option (1-7): ");

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

            if (option == 6)
            {
                DemonstrateWeightAddition();
                return;
            }

            DemonstrateGenericQuantityUc10();
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

                if (int.TryParse(input, out int option) && option >= 1 && option <= 7)
                    return option;

                Console.WriteLine("Invalid option. Enter a value from 1 to 7.");
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

        private static bool DemonstrateEquality<TUnit>(Quantity<TUnit> first, Quantity<TUnit> second)
            where TUnit : struct, Enum
        {
            return first.Equals(second);
        }

        private static Quantity<TUnit> DemonstrateConversion<TUnit>(Quantity<TUnit> quantity, TUnit targetUnit)
            where TUnit : struct, Enum
        {
            return quantity.ConvertTo(targetUnit);
        }

        private static Quantity<TUnit> DemonstrateAddition<TUnit>(Quantity<TUnit> first, Quantity<TUnit> second)
            where TUnit : struct, Enum
        {
            return first.Add(second);
        }

        private static Quantity<TUnit> DemonstrateAddition<TUnit>(Quantity<TUnit> first, Quantity<TUnit> second, TUnit targetUnit)
            where TUnit : struct, Enum
        {
            return first.Add(second, targetUnit);
        }

        private static Quantity<TUnit> DemonstrateSubtraction<TUnit>(Quantity<TUnit> first, Quantity<TUnit> second)
            where TUnit : struct, Enum
        {
            return first.Subtract(second);
        }

        private static Quantity<TUnit> DemonstrateSubtraction<TUnit>(Quantity<TUnit> first, Quantity<TUnit> second, TUnit targetUnit)
            where TUnit : struct, Enum
        {
            return first.Subtract(second, targetUnit);
        }

        private static double DemonstrateDivision<TUnit>(Quantity<TUnit> first, Quantity<TUnit> second)
            where TUnit : struct, Enum
        {
            return first.Divide(second);
        }

        private static void DemonstrateGenericQuantityUc10()
        {
            Console.WriteLine("=== Generic Quantity Demo (UC10-UC14) ===");

            var lengthFeet = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var lengthInch = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
            Console.WriteLine($"Length equality: {DemonstrateEquality(lengthFeet, lengthInch)}");

            var convertedLength = DemonstrateConversion(lengthFeet, LengthUnit.Inch);
            Console.WriteLine($"Length conversion: {convertedLength}");

            var totalLength = DemonstrateAddition(lengthFeet, lengthInch, LengthUnit.Feet);
            Console.WriteLine($"Length addition: {totalLength}");

            var differenceLength = DemonstrateSubtraction(lengthFeet, lengthInch, LengthUnit.Feet);
            Console.WriteLine($"Length subtraction: {differenceLength}");

            var ratioLength = DemonstrateDivision(lengthFeet, new Quantity<LengthUnit>(6.0, LengthUnit.Inch));
            Console.WriteLine($"Length division ratio: {ratioLength:0.######}");

            var weightKg = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var weightGram = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);
            Console.WriteLine($"Weight equality: {DemonstrateEquality(weightKg, weightGram)}");

            var convertedWeight = DemonstrateConversion(weightKg, WeightUnit.Pound);
            Console.WriteLine($"Weight conversion: {convertedWeight}");

            var totalWeight = DemonstrateAddition(weightKg, weightGram, WeightUnit.Kilogram);
            Console.WriteLine($"Weight addition: {totalWeight}");

            var differenceWeight = DemonstrateSubtraction(weightKg, new Quantity<WeightUnit>(500.0, WeightUnit.Gram), WeightUnit.Kilogram);
            Console.WriteLine($"Weight subtraction: {differenceWeight}");

            var ratioWeight = DemonstrateDivision(weightKg, new Quantity<WeightUnit>(500.0, WeightUnit.Gram));
            Console.WriteLine($"Weight division ratio: {ratioWeight:0.######}");

            var volumeLitre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var volumeMilli = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
            Console.WriteLine($"Volume equality: {DemonstrateEquality(volumeLitre, volumeMilli)}");

            var convertedVolume = DemonstrateConversion(volumeLitre, VolumeUnit.Gallon);
            Console.WriteLine($"Volume conversion: {convertedVolume}");

            var totalVolume = DemonstrateAddition(volumeLitre, volumeMilli, VolumeUnit.Litre);
            Console.WriteLine($"Volume addition: {totalVolume}");

            var differenceVolume = DemonstrateSubtraction(volumeLitre, new Quantity<VolumeUnit>(250.0, VolumeUnit.Millilitre), VolumeUnit.Millilitre);
            Console.WriteLine($"Volume subtraction: {differenceVolume}");

            var ratioVolume = DemonstrateDivision(volumeLitre, new Quantity<VolumeUnit>(500.0, VolumeUnit.Millilitre));
            Console.WriteLine($"Volume division ratio: {ratioVolume:0.######}");

            var tempCelsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            var tempFahrenheit = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);
            Console.WriteLine($"Temperature equality (0C == 32F): {DemonstrateEquality(tempCelsius, tempFahrenheit)}");

            var convertedTemp = DemonstrateConversion(new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius), TemperatureUnit.Fahrenheit);
            Console.WriteLine($"Temperature conversion: {convertedTemp}");

            try
            {
                _ = tempCelsius.Add(new Quantity<TemperatureUnit>(10.0, TemperatureUnit.Celsius));
            }
            catch (UnsupportedOperationException exception)
            {
                Console.WriteLine($"Temperature addition blocked: {exception.Message}");
            }
        }
    }
}