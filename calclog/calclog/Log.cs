using System;

namespace calclog
{
    public sealed class Log
    {
        private string type { get; }

        public Log(Type type) => this.type = nameof(type);

        public void info(string text, bool hasPrefix = false) => logger("Info", text, hasPrefix, ConsoleColor.Gray);

        public void warning(string text, bool hasPrefix = false) => logger("Warning", text, hasPrefix, ConsoleColor.Yellow);

        public void error(string text, bool hasPrefix = false) => logger("Error", text, hasPrefix, ConsoleColor.Red);

        private void logger(string prefix, string text, bool hasPrefix, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(string.Format("{0}{1}{2}", hasPrefix ? $"({prefix})   " : "", hasPrefix ? $"{type}   " : "", text));
            Console.ResetColor();
        }
    }
}
