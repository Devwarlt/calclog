using System;

namespace calclog.BooleanUtils
{
    /// <summary>
    /// Represents a structure for <see cref="Calculator.CalculatorHandler.operatorActions"/> dictionary.
    /// </summary>
    public struct BooleanOperation
    {
        public BooleanOperation(char symbol, ConsoleKey[] keys)
        {
            this.symbol = symbol;
            this.keys = keys;
        }

        /// <summary>
        /// Allowed <see cref="ConsoleKey"/> to trigger <see cref="symbol"/> on <see cref="Console"/>.
        /// </summary>
        public ConsoleKey[] keys { get; }

        /// <summary>
        /// Represents an unicode symbol for <see cref="BooleanOperation"/>.
        /// </summary>
        public char symbol { get; }
    }
}
