using System;

namespace calclog.utils.BooleanUtils
{
    public struct BooleanOperation
    {
        public char symbol { get; }
        public ConsoleKey[] keys { get; }
        public Func<string, string> calculate { get; }

        public BooleanOperation(char symbol, ConsoleKey[] keys, Func<string, string> calculate)
        {
            this.symbol = symbol;
            this.keys = keys;
            this.calculate = calculate;
        }
    }
}
