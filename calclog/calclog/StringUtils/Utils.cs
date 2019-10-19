using calclog.ShuntingYardAlgorithm;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace calclog.StringUtils
{
    using bs = BooleanSymbol;

    /// <summary>
    /// Represents an utility class for strings.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Filter an expression based on a <see cref="Regex"/> expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string filter(this string expression, string pattern, string replacement)
            => Regex.Replace(expression, pattern, replacement);

        /// <summary>
        /// Gets all premisses from an expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Dictionary<char, int> getPremises(this string expression)
        {
            var premises = new Dictionary<char, int>();

            for (var i = 0; i < expression.filter("[^A-Z]", ".").Length; i++)
                if (expression[i] == '.') continue;
                else
                {
                    if (premises.ContainsKey(expression[i])) premises[expression[i]]++;
                    else premises.Add(expression[i], 1);
                }

            return premises;
        }

        /// <summary>
        /// Convert a string on <see cref="bs"/> collection using <see cref="Regex"/> expression
        /// to filter specific characters from <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<bs> getSymbols(this string value)
        {
            var @string = Regex.Replace(value, @"\s+", "");

            foreach (var @char in @string)
            {
                if (char.IsLetter(@char) && !bs.expressionSymbols.Keys.Any(c => c.@char == @char))
                {
                    yield return new bs(char.ToUpperInvariant(@char), Token.Premise);
                    continue;
                }

                if (bs.expressionSymbols.Keys.Any(c => c.@char == @char))
                    yield return bs.expressionSymbols[(@char, bs.getItemBySymbol(@char))].Invoke(@char);
            }
        }

        /// <summary>
        /// Split upper case occurencies on string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] splitUpperCase(this string value)
            => Regex.Split(value, @"(?<!^)(?=[A-Z])");

        /// <summary>
        /// Split an array of strings and join within a generic separator.
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string toStringSeparator(this IEnumerable<string> enumerator, string separator)
            => string.Join(separator, enumerator);

        /// <summary>
        /// Convert a bool type on formatted char to being displayed at truth table.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static char toTruthTableFormat(this bool value)
            => value ? 'T' : 'F';

        /// <summary>
        /// Gets an upper case separator on string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string toUpperCaseSeparator(this string value)
            => string.Join(" ", value.splitUpperCase());
    }
}
