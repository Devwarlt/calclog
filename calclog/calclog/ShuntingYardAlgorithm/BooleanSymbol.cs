using System;
using System.Collections.Generic;
using System.Linq;

namespace calclog.ShuntingYardAlgorithm
{
    using bi = BooleanItem;
    using bs = BooleanSymbol;
    using t = Token;

    /// <summary>
    /// Represents a class to manage expression symbols and according actions
    /// when invoked from boolean expression during resolution.
    /// </summary>
    public class BooleanSymbol
    {
        /// <summary>
        /// A dictionary of boolean operators and symbols (except premises).
        /// </summary>
        public static Dictionary<(char @char, bi item), Func<char, bs>> expressionSymbols
            = new Dictionary<(char @char, bi item), Func<char, bs>>()
            {
                { ('0', bi.False), (@char) => new bs(@char, false) },
                { ('1', bi.True), (@char) => new bs(@char, true) },
                { ('(', bi.LeftParenthesis), (@char) => new bs(@char, t.LeftParenthesis) },
                { (')', bi.RightParenthesis), (@char) => new bs(@char, t.RightParenthesis) },
                { ('\u00AC', bi.NegationOperator), (@char) => new bs(@char, t.Operator, 4, 1, false) },
                { ('\u2194', bi.BiconditionalOperator), (@char) => new bs(@char, t.Operator, 3, 2) },
                { ('\u2192', bi.ConditionalOperator), (@char) => new bs(@char, t.Operator, 3, 2) },
                { ('x', bi.ExclusiveDisjunctionOperator), (@char) => new bs(@char, t.Operator, 2, 2) },
                { ('^', bi.ConjunctionOperator), (@char) => new bs(@char, t.Operator, 1, 2) },
                { ('v', bi.DisjunctionOperator), (@char) => new bs(@char, t.Operator, 1, 2) }
            };

        public BooleanSymbol(
                    char symbol, t token = t.Unknown, int precedence = -1,
                    int requiredArguments = 0, bool leftAssociate = true)
        {
            this.symbol = symbol;
            this.token = token;
            this.precedence = precedence;
            this.requiredArguments = requiredArguments;
            this.leftAssociate = leftAssociate;
        }

        public BooleanSymbol(char symbol, bool booleanValue)
        {
            this.symbol = symbol;
            this.booleanValue = booleanValue;
        }

        /// <summary>
        /// Value that <see cref="bs"/> does assume along <see cref="BooleanUtils.TruthTable.handleExpression(string)"/>
        /// generation.
        /// </summary>
        public bool booleanValue { get; set; }

        /// <summary>
        /// Only variable if <see cref="token"/> is <see cref="t.Premise"/> flag.
        /// </summary>
        public bool isVariable => token == t.Premise;

        /// <summary>
        /// Represents if symbol has left / right association dependencies.
        /// </summary>
        public bool leftAssociate { get; }

        /// <summary>
        /// Precedence of symbol along execution.
        /// </summary>
        public int precedence { get; }

        /// <summary>
        /// Represents a number of dependencies required to execute this symbol based on
        /// <see cref="t"/>.
        /// </summary>
        public int requiredArguments { get; }

        /// <summary>
        /// A <see cref="char"/> that represents current symbol.
        /// </summary>
        public char symbol { get; }

        /// <summary>
        /// Flag of token for symbol.
        /// </summary>
        public t token { get; }

        /// <summary>
        /// Gets <see cref="bi"/> by <see cref="char"/> from <see cref="expressionSymbols"/> dictionary.
        /// </summary>
        /// <param name="char"></param>
        /// <returns></returns>
        public static bi getItemBySymbol(char @char)
            => expressionSymbols.Keys.First(entry => entry.@char == @char).item;

        /// <summary>
        /// Gets <see cref="char"/> by <see cref="bi"/> from <see cref="expressionSymbols"/> dictionary.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static char getSymbolByItem(bi item)
            => expressionSymbols.Keys.First(entry => entry.item == item).@char;
    }
}
