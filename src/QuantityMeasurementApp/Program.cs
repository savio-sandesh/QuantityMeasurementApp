using System;
using QuantityMeasurementApp.Controllers;
using BusinessLayer;
using RepositoryLayer;
using ModelLayer;
using UtilityLayer;
using QuantityMeasurementDomain.Units;

namespace QuantityMeasurementApp
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Logger.Info("Application starting...");

                var repositoryType = DatabaseConfig.GetRepositoryType();
                if (repositoryType.Equals(RepositoryTypeConstants.Database, StringComparison.OrdinalIgnoreCase))
                {
                    DatabaseInitializer.Initialize();
                    Logger.Info("Database initialized successfully");
                }
                else
                {
                    Logger.Info($"Repository mode '{repositoryType}' detected; skipping SQL initialization.");
                }

                IConsoleMenu consoleMenu = new ConsoleMain();
                consoleMenu.DisplayMenu();

                Logger.Info("Application ended");
            }
            catch (Exception ex)
            {
                Logger.Error("Unhandled exception: " + ex);
            }
        }
    }
}