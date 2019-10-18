using calclog.BooleanUtils;
using calclog.StringUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static calclog.App;

namespace calclog.Calculator
{
    using bo = BooleanOperator;
    using ck = ConsoleKey;
    using cs = Console;

    public sealed class CalculatorHandler
    {
        private Dictionary<bo, BooleanOperation> operatorActions;
        private Dictionary<ck, char> extraActions;
        private List<ck> allowedKeys;
        private List<ck> letterKeys;
        private List<ck> optionKeys;
        private List<ck> deletionKeys;
        private List<ck> parenthesisKeys;
        private ck executionKey;
        private Thread thread { get; }
        private string expression = string.Empty;
        private bool hasParenthesis = false;
        private bool isMissingParenthesis = false;

        public CalculatorHandler() => thread = new Thread(handleCalculator);

        public void configureKeys()
        {
            operatorActions = new Dictionary<bo, BooleanOperation>()
            {
                { bo.Negation, new BooleanOperation('\u00AC', new[] { ck.NumPad1, ck.D1 }, (exp) => negation(exp)) },
                { bo.Conjunction, new BooleanOperation('^', new[] {  ck.NumPad2, ck.D2 }, (exp) => conjunction(exp)) },
                { bo.Disjunction, new BooleanOperation('v', new[] { ck.NumPad3, ck.D3 }, (exp) => disjunction(exp)) },
                { bo.ExclusiveDisjunction, new BooleanOperation('x', new[] { ck.NumPad4, ck.D4 }, (exp) => exclusiveDisjunction(exp)) },
                { bo.Conditional, new BooleanOperation('\u2192', new[] { ck.NumPad5, ck.D5 }, (exp) => conditional(exp)) },
                { bo.Biconditional, new BooleanOperation('\u2194', new[] { ck.NumPad6, ck.D6 }, (exp) => biconditional(exp)) }
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

        public BooleanOperation getOperation(ck key) => operatorActions.Values.First(op => op.keys.Contains(key));

        private string negation(string exp)
        {
            // TODO.
            return "negation operation";
        }

        private string conjunction(string exp)
        {
            // TODO.
            return "conjunction operation";
        }

        private string disjunction(string exp)
        {
            // TODO.
            return "disjunction operation";
        }

        private string exclusiveDisjunction(string exp)
        {
            // TODO.
            return "exclusive disjunction operation";
        }

        private string conditional(string exp)
        {
            // TODO.
            return "conditional operation";
        }

        private string biconditional(string exp)
        {
            // TODO.
            return "biconditional operation";
        }

        private bool isEmpty(out int count)
        {
            count = -1;

            if (string.IsNullOrEmpty(expression)) return true;

            var filters = operatorActions.Values.Select(op => op.symbol).ToList();
            filters.AddRange(extraActions.Values);

            var filteredExpression = expression;

            foreach (var filter in filters)
                filteredExpression = filteredExpression.Replace(filter, '.');

            var premisses = filteredExpression.getPremises();

            count = premisses.Count;

            return count == 0;
        }

        public void startThread() => thread.Start();

        public void stopThread() => thread.Abort();

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

        private void handleCalculator()
        {
            displayOptions();

            var readKey = cs.ReadKey();

            handleKeys(readKey.Key);
        }

        private void handleKeys(ck key)
        {
            if (allowedKeys.Contains(key))
            {
                if (letterKeys.Contains(key)) expression += key.ToString();
                if (optionKeys.Contains(key)) expression += getOperation(key).symbol;
                if (deletionKeys.Contains(key) && !string.IsNullOrEmpty(expression))
                    expression = expression.Remove(expression.Length - 1);
                if (parenthesisKeys.Contains(key)) expression += extraActions[key];

                handleParenthesis();

                if (executionKey == key)
                {
                    if (!isEmpty(out int count))
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
                        handleExecution(count);
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

        private void handleParenthesis()
        {
            hasParenthesis = extraActions.Values
                .Any(par => expression.Contains(par));
            isMissingParenthesis = extraActions.Values
                .Select(par => expression.Count(chr => chr == par))
                .Distinct().ToList().Count > 1;
        }

        private void handleExecution(int count)
        {
            cs.Clear();

            displayTitle();

            log.warning(
                string.Format("- Number of premisses: {0}\n- Number of results: {1}",
                    count, Math.Pow(2, count)
                ));
            log.info("\n");
            log.info("Press any key to continue...");
            cs.ReadKey(true);
            cs.Clear();

            handleCalculator();
        }
    }
}
