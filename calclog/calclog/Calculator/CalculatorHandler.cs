using calclog.BooleanUtils;
using calclog.ShuntingYardAlgorithm;
using calclog.StringUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static calclog.App;

namespace calclog.Calculator
{
    using bi = BooleanItem;
    using bs = BooleanSymbol;
    using ck = ConsoleKey;
    using cs = Console;

    /// <summary>
    /// Represents a class to manage all features of calclog.
    /// </summary>
    public sealed class CalculatorHandler
    {
        /// <summary>
        /// Allowed keys on <see cref="cs"/>.
        /// </summary>
        private List<ck> allowedKeys;

        /// <summary>
        /// Allowed deletion keys at <see cref="cs"/>.
        /// </summary>
        private List<ck> deletionKeys;

        /// <summary>
        /// Allowed execution key at <see cref="cs"/>.
        /// </summary>
        private ck executionKey;

        /// <summary>
        /// Represents an input that will be converted into a valid boolean
        /// expression throught <see cref="handleExecution"/> invoke.
        /// </summary>
        private string expression = string.Empty;

        /// <summary>
        /// An extra dictionary of valid actions along invoke on <see cref="cs"/>.
        /// </summary>
        private Dictionary<ck, char> extraActions;

        /// <summary>
        /// Represents a variable if <see cref="expression"/> contains
        /// parenthesis.
        /// </summary>
        private bool hasParenthesis = false;

        /// <summary>
        /// Represents a variable if <see cref="expression"/> has
        /// missing parenthesis, this alog has dependency with <see cref="hasParenthesis"/>
        /// property.
        /// </summary>
        private bool isMissingParenthesis = false;

        /// <summary>
        /// Allowed keys for letters at <see cref="cs"/>.
        /// </summary>
        private List<ck> letterKeys;

        /// <summary>
        /// A dictionary of valid operators and actions along invoke on <see cref="cs"/>.
        /// </summary>
        private Dictionary<bi, BooleanOperation> operatorActions;

        /// <summary>
        /// Allowed options keys at <see cref="cs"/>.
        /// </summary>
        private List<ck> optionKeys;

        /// <summary>
        /// Allowed parenthesis keys at <see cref="cs"/>.
        /// </summary>
        private List<ck> parenthesisKeys;

        /// <summary>
        /// Represents a truth-table instance.
        /// </summary>
        private TruthTable truthTable;

        public CalculatorHandler()
        {
            truthTable = new TruthTable();
            thread = new Thread(handleCalculator);
        }

        /// <summary>
        /// Internal thread responsible for <see cref="handleCalculator"/> method.
        /// </summary>
        private Thread thread { get; }

        /// <summary>
        /// Configure all keys that will be allowed at <see cref="cs"/>.
        /// </summary>
        public void configureKeys()
        {
            var operators = new Dictionary<bi, char>();

            foreach (var (@char, item) in bs.expressionSymbols.Keys)
                operators.Add(item, @char);

            operatorActions = new Dictionary<bi, BooleanOperation>
            {
                { bi.NegationOperator, new BooleanOperation(operators[bi.NegationOperator], new[] { ck.NumPad1, ck.D1 }) },
                { bi.ConjunctionOperator, new BooleanOperation(operators[bi.ConjunctionOperator], new[] {  ck.NumPad2, ck.D2 }) },
                { bi.DisjunctionOperator, new BooleanOperation(operators[bi.DisjunctionOperator], new[] { ck.NumPad3, ck.D3 }) },
                { bi.ExclusiveDisjunctionOperator, new BooleanOperation(operators[bi.ExclusiveDisjunctionOperator], new[] { ck.NumPad4, ck.D4 }) },
                { bi.ConditionalOperator, new BooleanOperation(operators[bi.ConditionalOperator], new[] { ck.NumPad5, ck.D5 }) },
                { bi.BiconditionalOperator, new BooleanOperation(operators[bi.BiconditionalOperator], new[] { ck.NumPad6, ck.D6 }) }
            };

            extraActions = new Dictionary<ck, char>()
            {
                { ck.D0, ')' },
                { ck.D9, '(' }
            };

            letterKeys = new List<ck>()
            {
                ck.A, ck.B, ck.C, ck.D, ck.E, ck.F, ck.G, ck.H, ck.I, ck.J, ck.K, ck.L, ck.M,
                ck.N, ck.O, ck.P, ck.Q, ck.R, ck.S, ck.T, ck.U, ck.V, ck.X, ck.Y, ck.Z, ck.W
            };

            optionKeys = operatorActions.Values.SelectMany(vars => vars.keys).ToList();

            deletionKeys = new List<ck>() { ck.Backspace, ck.Delete };

            parenthesisKeys = extraActions.Keys.ToList();

            executionKey = ck.Enter;

            allowedKeys = new List<ck>();
            allowedKeys.AddRange(letterKeys);
            allowedKeys.AddRange(optionKeys);
            allowedKeys.AddRange(deletionKeys);
            allowedKeys.AddRange(parenthesisKeys);
            allowedKeys.Add(executionKey);
        }

        /// <summary>
        /// Gets <see cref="BooleanOperation"/> based on <paramref name="key"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BooleanOperation getOperation(ck key) => operatorActions.Values.First(op => op.keys.Contains(key));

        /// <summary>
        /// Initialize <see cref="thread"/> process.
        /// </summary>
        public void startThread() => thread.Start();

        /// <summary>
        /// Abort <see cref="thread"/> process.
        /// </summary>
        public void stopThread() => thread.Abort();

        /// <summary>
        /// Display all valid options at <see cref="cs"/>.
        /// </summary>
        private void displayOptions()
        {
            displayTitle();

            log.info("Valid boolean operators:");
            log.info("\n");

            foreach (var op in operatorActions)
                log.info(
                    string.Format("\t['{0}']. {1}: {2}",
                        op.Value.keys.Select(key => key.ToString()).ToList().toStringSeparator("' or '"),
                        op.Key.ToString().toUpperCaseSeparator(),
                        op.Value.symbol
                    ));

            log.info("\n");

            foreach (var extra in extraActions)
                log.info(
                    string.Format("\t['{0}']: {1}",
                        extra.Key.ToString(),
                        extra.Value
                    ));

            log.info(string.Format("\t['{0}']. Execute expression.", executionKey.ToString()));
            log.info(
                string.Format("\t['{0}']. Remove last addition.",
                    deletionKeys.Select(key => key.ToString()).ToList().toStringSeparator("' or '")
                ));
            log.info("\n");
            log.warning(
                string.Format("{0}",
                    hasParenthesis && isMissingParenthesis ? "Missing parenthesis on expression!" : ""
                ));
            log.info("\n");
            log.info(
                string.Format("Expression: {0}",
                    string.IsNullOrEmpty(expression) ? "empty" : expression
                ));
        }

        /// <summary>
        /// Display the ASCII art of calclog at <see cref="cs"/>.
        /// </summary>
        private void displayTitle()
            => log.info(
@"
            _      _
   ___ __ _| | ___| | ___   __ _
  / __/ _` | |/ __| |/ _ \ / _` |
 | (_| (_| | | (__| | (_) | (_| |
  \___\__,_|_|\___|_|\___/ \__, |
                           |___/
");

        /// <summary>
        /// Main method responsible to invoke <see cref="handleKeys(ck)"/>.
        /// </summary>
        private void handleCalculator()
        {
            displayOptions();

            var readKey = cs.ReadKey();

            handleKeys(readKey.Key);
        }

        /// <summary>
        /// Handle execution to convert <see cref="expression"/>
        /// into a valid boolean expression via <see cref="truthTable"/>
        /// methods and dependencies.
        /// </summary>
        private void handleExecution()
        {
            cs.Clear();

            displayTitle();

            if (!truthTable.handleExpression(expression))
            {
                handleCalculator();
                return;
            }

            log.info("\n");
            log.info("Press any key to continue...");
            cs.ReadKey(true);
            cs.Clear();

            handleCalculator();
        }

        /// <summary>
        /// Handle <see cref="ck"/> based on its own dependency.
        /// </summary>
        /// <param name="key"></param>
        private void handleKeys(ck key)
        {
            if (allowedKeys.Contains(key))
            {
                if (letterKeys.Contains(key)) expression += key.ToString().ToUpperInvariant();
                if (optionKeys.Contains(key)) expression += getOperation(key).symbol;
                if (deletionKeys.Contains(key) && !string.IsNullOrEmpty(expression))
                    expression = expression.Remove(expression.Length - 1);
                if (parenthesisKeys.Contains(key)) expression += extraActions[key];

                handleParenthesis();

                if (executionKey == key)
                {
                    if (!isEmpty())
                    {
                        if (hasParenthesis)
                        {
                            if (isMissingParenthesis)
                            {
                                log.error("\r\nExpression is invalid due missing parenthesis statement!");
                                log.info("\n");
                                log.warning("Press any key to continue...");
                                cs.ReadKey(true);
                                cs.Clear();

                                handleCalculator();
                                return;
                            }
                        }
                        handleExecution();
                        return;
                    }
                    else
                    {
                        log.error("\r\nExpression is empty!");
                        log.info("\n");
                        log.warning("Press any key to continue...");
                        cs.ReadKey(true);
                    }
                }
            }
            else
            {
                log.error($"\rConsole key '{key.ToString()}' isn't allowed!");
                log.info("\n");
                log.warning("Press any key to continue...");
                cs.ReadKey(true);
            }

            cs.Clear();

            handleCalculator();
        }

        /// <summary>
        /// Handle parenthesis and variables <see cref="hasParenthesis"/> and
        /// <see cref="isMissingParenthesis"/>.
        /// </summary>
        private void handleParenthesis()
        {
            hasParenthesis = extraActions.Values
                .Any(par => expression.Contains(par));
            isMissingParenthesis = extraActions.Values
                .Select(par => expression.Count(chr => chr == par))
                .Distinct().ToList().Count > 1;
        }

        /// <summary>
        /// Check if <see cref="expression"/> is empty or null.
        /// </summary>
        /// <returns></returns>
        private bool isEmpty()
        {
            if (string.IsNullOrEmpty(expression)) return true;

            var filters = operatorActions.Values.Select(op => op.symbol).ToList();
            filters.AddRange(extraActions.Values);

            var filteredExpression = expression;

            foreach (var filter in filters)
                filteredExpression = filteredExpression.Replace(filter, '.');

            return filteredExpression.getPremises().Count == 0;
        }
    }
}
