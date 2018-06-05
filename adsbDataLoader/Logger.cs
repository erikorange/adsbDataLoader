using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adsbDataLoader
{
    class Logger
    {
        public static void log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        public static void logNoCr(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
        }

        public static void logSameLineNoCr(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write("\r" + message);
        }

        public static void log(string message, ConsoleColor color, bool readKey)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
