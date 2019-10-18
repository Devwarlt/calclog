using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace calclog.StringUtils
{
    public static class Utilities
    {
        /// <summary>
        /// Split upper case occurencies on string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] splitUpperCase(this string value)
            => Regex.Split(value, @"(?<!^)(?=[A-Z])");

        /// <summary>
        /// Gets an upper case separator on string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string toUpperCaseSeparator(this string value)
            => string.Join(" ", value.splitUpperCase());

        /// <summary>
        /// Split an array of strings and join within a generic separator.
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string toStringSeparator(this IEnumerable<string> enumerator, string separator)
            => string.Join(separator, enumerator);

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
    }
}
