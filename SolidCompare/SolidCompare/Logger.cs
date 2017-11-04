using System;

namespace SolidCompare
{
    class Logger
    {
        public static void Info(string message)
        {
            Console.WriteLine("[INFO] " + message);
        }

        public static void Warn(string message)
        {
            Console.WriteLine("[WARNING] " + message);
        }

        public static void Error(string className, string methodName, string message)
        {
            Console.WriteLine("[ERROR] " + message + " in method: " + methodName + " of class: " + className);
        }
    }
}
