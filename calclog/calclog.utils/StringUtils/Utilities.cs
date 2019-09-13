using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace calclog.utils.StringUtils
{
    public static class Utilities
    {
        public static string Filter(this string expression, string pattern, string replacement)
            => Regex.Replace(expression, pattern, replacement);

        public static Dictionary<char, List<int>> GetPremises(this string expression)
        {
            var premises = new Dictionary<char, List<int>>();

            for (var i = 0; i < expression.Filter("[^a-z]", ".").Length; i++)
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