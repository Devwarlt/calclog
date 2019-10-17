using System;
using System.Globalization;
using System.Threading;

namespace calclog.terminal
{
    // https://www.geeksforgeeks.org/evaluate-a-boolean-expression-represented-as-string/
    public static class App
    {
        private static readonly ManualResetEvent shutdown = new ManualResetEvent(false);
        private static Thread handler;

        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            Console.Title = "calclog - Terminal";

            handler = new Thread(ConsoleHandler);
            handler.Start();

            Console.CancelKeyPress += delegate { shutdown.Set(); };

            shutdown.WaitOne();

            handler.Abort();

            Environment.Exit(0);
        }

        private static void ConsoleHandler()
        {
            Console.WriteLine("Type something...\n");

            var input = Console.ReadLine();

            Console.WriteLine("\n");
            Console.WriteLine("Input: " + input);
            Console.WriteLine("\n\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();

            ConsoleHandler();
        }
    }
}
