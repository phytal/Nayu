using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nayu
{
    public class Logger
    {
        internal static Task Log(LogMessage logMessage)
        {
            Console.ForegroundColor = SeverityToConsoleColor(logMessage.Severity);
            var message = string.Concat(DateTime.Now.ToShortTimeString(), " [", logMessage.Source, "] ",
                logMessage.Message);
            Console.WriteLine(message);
            Console.ResetColor();
            return Task.CompletedTask;
        }

        /// <summary>Console logging event for music.</summary>
        public void ConsoleMusicLog(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nMusic Service:" +
                              $"\nMessage: [\"{msg}\"]\n");
        }

        /// <summary>Console logging event for general commands.</summary>
        public void ConsoleCommandLog(string msg, LogSeverity severity)
        {
            Console.ForegroundColor = SeverityToConsoleColor(severity);
            Console.WriteLine($"\nCommand Service:" +
                              $"\nMessage: [\"{msg}\"]\n");
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