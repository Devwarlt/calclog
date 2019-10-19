using calclog.ShuntingYardAlgorithm;
using calclog.StringUtils;
using System;
using static calclog.App;

namespace calclog.BooleanUtils
{
    using be = BooleanExpression;
    using cs = Console;

    /// <summary>
    /// Represents a class to manage truth table generations and boolean
    /// expression handlers along format.
    /// </summary>
    public class TruthTable
    {
        private be expression;

        /// <summary>
        /// Handle an input expression and generate a truth table based on relative boolean values for
        /// each premise within boolean expression. It does use Djikstra's Shunting Yard algorithm
        /// on RPN (Reverser Polish Notation).
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool handleExpression(string input)
        {
            log.info("Generating truth table...");

            expression = new be(input);
            expression.configure();

            var premises = expression.getPremises();
            var count = premises.Count;
            var result = (int)Math.Pow(2, count);

            log.warning(
                string.Format("\t- {0} premise{1}\n\t- {2} result{3}",
                    count, count > 1 ? "s" : "",
                    result, result > 1 ? "s" : ""
                ));

            var table = generateTable(premises.Count);

            log.info("\n");

            var tableHeader = string.Empty;

            foreach (var premise in premises)
                tableHeader += string.Format(" {0} |", premise);

            tableHeader += " Result\n";

            for (var i = 0; i <= count; i++)
                tableHeader += "-------";

            log.info(tableHeader);

            var answer = new bool[table.GetLength(0)];

            for (var i = 0; i < table.GetLength(0); i++)
            {
                for (var j = 0; j < table.GetLength(1); j++)
                    expression.setPremiseValue(premises[j], table[i, j]);

                try { answer[i] = expression.solveExpression(); }
                catch
                {
                    log.error("\nExpression is invalid due missing operator statement!");
                    log.info("\n");
                    log.warning("Press any key to continue...");
                    cs.ReadKey(true);
                    cs.Clear();

                    return false;
                }
            }

            var tableBody = string.Empty;

            for (var i = 0; i < table.GetLength(0); i++)
            {
                for (var j = 0; j < table.GetLength(1); j++)
                    tableBody += string.Format(" {0} |", table[i, j].toTruthTableFormat());

                tableBody += string.Format(" {0}\n", answer[i].toTruthTableFormat());
            }

            log.info(tableBody);

            return true;
        }

        /// <summary>
        /// Generate a bi-dimensional matrix for truth table.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private bool[,] generateTable(int column)
        {
            var row = (int)Math.Pow(2, column);
            var table = new bool[row, column];
            var divider = row;

            for (var y = 0; y < column; y++)
            {
                divider /= 2;

                var entry = false;

                for (var x = 0; x < row; x++)
                {
                    table[x, y] = entry;

                    if (divider == 1 || (x + 1) % divider == 0)
                        entry = !entry;
                }
            }

            return table;
        }
    }
}
