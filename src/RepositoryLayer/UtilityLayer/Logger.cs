using System;

namespace UtilityLayer
{
    public static class Logger
    {
        public static void Info(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.Now} - {message}");
        }

        public static void Error(string message)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now} - {message}");
        }

        public static void Debug(string message)
        {
            Console.WriteLine($"[DEBUG] {DateTime.Now} - {message}");
        }
    }
}