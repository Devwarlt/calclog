using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace calclog.utils.StringUtils
{
    public static class Utilities
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
        public static Dictionary<char, List<int>> getPremises(this string expression)
        {
            var premises = new Dictionary<char, List<int>>();

            for (var i = 0; i < expression.filter("[^a-z]", ".").Length; i++)
                if (expression[i] == '.') continue;
                else
                {
                    if (premises.ContainsKey(expression[i])) premises[expression[i]].Add(i);
                    else premises.Add(expression[i], new List<int>() { i });
                }

            return premises;
        }
    }
}
