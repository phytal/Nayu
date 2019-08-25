using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nayu
{
    internal class Logger
    {
        internal static Task Log(LogMessage logMessage)
        {
            Console.ForegroundColor = SeverityToConsoleColor(logMessage.Severity);
            string message = String.Concat(DateTime.Now.ToShortTimeString(), " [", logMessage.Source, "] ", logMessage.Message);
            Console.WriteLine(message);
            Console.ResetColor();
            return Task.CompletedTask;
        }
        /// <summary>Console logging event for music.</summary>
        public void ConsoleMusicLog(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nMusic Service:" +
                              $"\n" +
                              $"\nMessage: [\"{msg}\"]");
        }
        private static ConsoleColor SeverityToConsoleColor(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.Red;
                case LogSeverity.Debug:
                    return ConsoleColor.Blue;
                case LogSeverity.Error:
                    return ConsoleColor.Yellow;
                case LogSeverity.Info:
                    return ConsoleColor.Blue;
                case LogSeverity.Verbose:
                    return ConsoleColor.Green;
                case LogSeverity.Warning:
                    return ConsoleColor.Magenta;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}