using calclog.Calculator;
using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace calclog
{
    public static class App
    {
        public static Log log = new Log(typeof(App));
        private static readonly ManualResetEvent shutdown = new ManualResetEvent(false);
        private static CalculatorHandler calculator;

        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            Console.Title = "calclog - Terminal";
            Console.InputEncoding = Encoding.UTF8;

            calculator = new CalculatorHandler();
            calculator.configureKeys();
            calculator.startThread();

            Console.CancelKeyPress += delegate { shutdown.Set(); };

            shutdown.WaitOne();

            calculator.stopThread();

            Environment.Exit(0);
        }
    }
}
