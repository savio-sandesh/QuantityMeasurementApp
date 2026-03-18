using System;
using QuantityMeasurementApp.Controllers;
using BusinessLayer;
using RepositoryLayer;
using ModelLayer;
using QuantityMeasurementDomain.Units;

namespace QuantityMeasurementApp
{
    internal class Program
    {
        private static void Main()
        {
           IConsoleMenu consoleMenu = new ConsoleMain();
           consoleMenu.DisplayMenu();
        }
    }
}