using System;
using BusinessLayer;
using ModelLayer;
using QuantityMeasurementApp.Controllers;
using QuantityMeasurementDomain.Units;
using RepositoryLayer;

namespace QuantityMeasurementApp
{
    public class ConsoleMain : IConsoleMenu
    {
        public void DisplayMenu()
        {
            IQuantityMeasurementRepository repository = QuantityMeasurementCacheRepository.Instance;
            IQuantityMeasurementService service = new QuantityMeasurementService(repository);
            var controller = new QuantityMeasurementController(service);

            PrintAppHeader();

            while (true)
            {
                PrintSectionHeader("Category Menu");
                PrintMenuOptions(
                    ("1", "Length"),
                    ("2", "Weight"),
                    ("3", "Volume"),
                    ("4", "Temperature"),
                    ("0", "Exit")
                );

                string category = ReadChoiceInput("Select a category", "0", "1", "2", "3", "4");

                if (category == "0")
                {
                    PrintInfo("Exiting application...");
                    return;
                }

                if (!TryGetMeasurementType(category, out string measurementType))
                {
                    PrintError("Invalid category.");
                    continue;
                }

                HandleOperations(controller, measurementType);
            }
        }

        private static void HandleOperations(QuantityMeasurementController controller, string? type)
        {
            bool isTemperature = string.Equals(type, "Temperature", StringComparison.OrdinalIgnoreCase);

            while (true)
            {
                PrintSectionHeader($"{type} Operations");

                if (isTemperature)
                {
                    PrintMenuOptions(
                        ("1", "Conversion"),
                        ("2", "Equality"),
                        ("3", "Back")
                    );
                }
                else
                {
                    PrintMenuOptions(
                        ("1", "Equality"),
                        ("2", "Conversion"),
                        ("3", "Addition"),
                        ("4", "Subtraction"),
                        ("5", "Division"),
                        ("6", "Back")
                    );
                }

                string op = isTemperature
                    ? ReadChoiceInput("Select an operation", "1", "2", "3")
                    : ReadChoiceInput("Select an operation", "1", "2", "3", "4", "5", "6");

                string backOption = isTemperature ? "3" : "6";
                if (op == backOption)
                {
                    return;
                }

                var q1 = ReadQuantity("first", type);
                if (q1 is null)
                {
                    continue;
                }

                bool needsSecondQuantity = isTemperature ? op == "2" : op != "2";
                var q2 = needsSecondQuantity ? ReadQuantity("second", type) : null;
                if (needsSecondQuantity && q2 is null)
                {
                    continue;
                }

                try
                {
                    switch (op)
                    {
                        case "1":
                            if (isTemperature)
                            {
                                string? target = ReadUnitInput(type, "target");
                                if (target is null)
                                {
                                    continue;
                                }

                                var converted = controller.Convert(q1, target);
                                PrintResult($"Converted: {converted.Value} {converted.Unit}");
                            }
                            else
                            {
                                bool result = controller.Compare(q1, q2!);
                                PrintResult($"Equal: {result}");
                            }
                            break;

                        case "2":
                            if (isTemperature)
                            {
                                bool result = controller.Compare(q1, q2!);
                                PrintResult($"Equal: {result}");
                            }
                            else
                            {
                                string? target = ReadUnitInput(type, "target");
                                if (target is null)
                                {
                                    continue;
                                }

                                var converted = controller.Convert(q1, target);
                                PrintResult($"Converted: {converted.Value} {converted.Unit}");
                            }
                            break;

                        case "3":
                            string? addTarget = ReadUnitInput(type, "target");
                            if (addTarget is null)
                            {
                                continue;
                            }

                            var sum = controller.Add(q1, q2!, addTarget);
                            PrintResult($"Result: {sum.Value} {sum.Unit}");
                            break;

                        case "4":
                            string? subtractTarget = ReadUnitInput(type, "target");
                            if (subtractTarget is null)
                            {
                                continue;
                            }

                            var difference = controller.Subtract(q1, q2!, subtractTarget);
                            PrintResult($"Result: {difference.Value} {difference.Unit}");
                            break;

                        case "5":
                            var ratio = controller.Divide(q1, q2!);
                            PrintResult($"Result: {ratio}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    PrintError(ex.Message);
                }
            }
        }

        private static bool TryGetMeasurementType(string? category, out string measurementType)
        {
            measurementType = category switch
            {
                "1" => "Length",
                "2" => "Weight",
                "3" => "Volume",
                "4" => "Temperature",
                _ => string.Empty
            };

            return !string.IsNullOrWhiteSpace(measurementType);
        }

        private static QuantityDTO? ReadQuantity(string label, string? type)
        {
            PrintSectionHeader($"Enter {label} quantity");

            Console.Write("Value: ");
            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                PrintError("Invalid number.");
                return null;
            }

            string? unit = ReadUnitInput(type, "unit");
            if (unit is null)
            {
                return null;
            }

            return new QuantityDTO(value, unit, type ?? "Length");
        }

        private static string? ReadUnitInput(string? type, string label)
        {
            var allowedUnits = GetAllowedUnits(type);
            if (allowedUnits.Length == 0)
            {
                PrintError("Invalid measurement type.");
                return null;
            }

            Console.WriteLine($"Available units: {string.Join(", ", allowedUnits)}");
            Console.Write($"Enter {label}: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                PrintError("Invalid unit.");
                return null;
            }

            if (double.TryParse(input, out _))
            {
                PrintError("Invalid unit. Please enter a unit name, not a number.");
                return null;
            }

            bool isKnownUnit = Array.Exists(allowedUnits, u => u.Equals(input, StringComparison.OrdinalIgnoreCase));
            if (!isKnownUnit)
            {
                PrintError("Invalid unit. Choose one of the available units.");
                return null;
            }

            return input;
        }

        private static void PrintAppHeader()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("        QUANTITY MEASUREMENT APPLICATION");
            Console.WriteLine("==================================================");
        }

        private static void PrintSectionHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {title} ---");
        }

        private static void PrintMenuOptions(params (string Key, string Label)[] options)
        {
            foreach (var option in options)
            {
                Console.WriteLine($" {option.Key}. {option.Label}");
            }
        }

        private static string ReadChoiceInput(string prompt, params string[] validChoices)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                string? choice = Console.ReadLine()?.Trim();

                if (choice is not null && Array.Exists(validChoices, v => v == choice))
                {
                    return choice;
                }

                PrintError("Invalid selection. Please choose a valid menu option.");
            }
        }

        private static void PrintResult(string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Result -> {message}");
        }

        private static void PrintError(string message)
        {
            Console.WriteLine($"[Error] {message}");
        }

        private static void PrintInfo(string message)
        {
            Console.WriteLine($"[Info] {message}");
        }

        private static string[] GetAllowedUnits(string? type)
        {
            return (type ?? string.Empty).ToLowerInvariant() switch
            {
                "length" => Enum.GetNames<LengthUnit>(),
                "weight" => Enum.GetNames<WeightUnit>(),
                "volume" => Enum.GetNames<VolumeUnit>(),
                "temperature" => Enum.GetNames<TemperatureUnit>(),
                _ => Array.Empty<string>()
            };
        }
    }
}
