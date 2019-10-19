using calclog.BooleanUtils;
using calclog.StringUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace calclog.ShuntingYardAlgorithm
{
    using bi = BooleanItem;
    using bops = BooleanOperations;
    using bs = BooleanSymbol;
    using t = Token;

    /// <summary>
    /// Represents a class to manage a boolean expression from string input.
    /// </summary>
    public class BooleanExpression
    {
        /// <summary>
        /// Represents a collection of <see cref="bs"/> which is part of a boolean expression.
        /// </summary>
        private List<bs> expression;

        public BooleanExpression(string input)
            => expression = new List<bs>(input.getSymbols());

        /// <summary>
        /// Configure missing statements on boolean expression, applying correction
        /// withing <see cref="insertMissingConjunctions"/> and <see cref="shuntingYard"/> algorithm.
        /// </summary>
        public void configure()
        {
            insertMissingConjunctions();

            expression = shuntingYard();
        }

        /// <summary>
        /// Gets all premises from <see cref="expression"/>.
        /// </summary>
        /// <returns></returns>
        public List<char> getPremises()
        {
            var premises = new List<char>();

            foreach (var element in expression)
                if (element.token == t.Premise && element.isVariable)
                    premises.Add(element.symbol);

            premises = premises.Distinct().ToList();
            premises.Sort();

            return premises;
        }

        /// <summary>
        /// Sets all occurrencies from <see cref="expression"/> for specific premise symbol.
        /// </summary>
        /// <param name="char"></param>
        /// <param name="value"></param>
        public void setPremiseValue(char @char, bool value)
        {
            foreach (var element in expression)
                if (element.symbol == @char)
                    element.booleanValue = value;
        }

        /// <summary>
        /// Solve <see cref="expression"/> as a boolean expression based on RPN.
        /// </summary>
        /// <returns></returns>
        public bool solveExpression()
        {
            var stack = new Stack<bool>();

            foreach (var element in expression)
            {
                if (element.token == t.Premise)
                    stack.Push(element.booleanValue);
                else if (element.token == t.Operator)
                {
                    if (element.requiredArguments > stack.Count) continue;

                    var item = bs.getItemBySymbol(element.symbol);

                    stack.Push(item == bi.NegationOperator ?
                        bops.operations[bi.NegationOperator].Invoke(stack.Pop(), false) :
                        bops.operations[item].Invoke(stack.Pop(), stack.Pop()));
                }
            }

            return stack.Pop();
        }

        /// <summary>
        /// Insert missing <see cref="bi.ConjunctionOperator"/> operators on <see cref="expression"/>;
        /// </summary>
        private void insertMissingConjunctions()
        {
            var current = 0;
            var next = 1;

            while (expression.Count > next)
            {
                var currentSymbol = expression[current];
                var nextSymbol = expression[next];

                if ((currentSymbol.token == t.Premise && nextSymbol.token == t.LeftParenthesis)
                    || (currentSymbol.token == t.Premise && nextSymbol.token == t.Premise)
                    || (currentSymbol.token == t.RightParenthesis && nextSymbol.token == t.LeftParenthesis)
                    || (currentSymbol.token == t.RightParenthesis && nextSymbol.token == t.Premise)
                    || (currentSymbol.symbol == bs.getSymbolByItem(bi.NegationOperator) && nextSymbol.token == t.Premise)
                    || (currentSymbol.symbol == bs.getSymbolByItem(bi.NegationOperator) && nextSymbol.token == t.LeftParenthesis))
                    expression.Insert(next, new bs(bs.getSymbolByItem(bi.ConjunctionOperator), t.Operator, 1));

                current++;
                next++;
            }
        }

        /// <summary>
        /// Execute Djikstra's Shunting Yard algorithm based on RPN.
        /// </summary>
        /// <returns></returns>
        private List<bs> shuntingYard()
        {
            var output = new List<bs>();
            var stack = new Stack<bs>();

            foreach (var element in expression)
            {
                if (element.token == t.Premise)
                    output.Add(element);
                else if (element.token == t.Operator)
                {
                    while (stack.Count > 0 && stack.Peek().token == t.Operator
                        && ((element.leftAssociate && element.precedence <= stack.Peek().precedence)
                            || (!element.leftAssociate && element.precedence < stack.Peek().precedence)))
                        output.Add(stack.Pop());

                    stack.Push(element);
                }
                else if (element.token == t.LeftParenthesis) stack.Push(element);
                else if (element.token == t.RightParenthesis)
                    try
                    {
                        while (stack.Peek().token != t.LeftParenthesis)
                            output.Add(stack.Pop());

                        stack.Pop();
                    }
                    catch (InvalidOperationException) { }

                while (stack.Count > 0)
                    output.Add(stack.Pop());
            }

            return output;
        }
    }
}
