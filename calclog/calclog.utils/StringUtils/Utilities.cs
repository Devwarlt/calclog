using calclog.utils.BooleanUtils;
using System.Linq;
using System.Text.RegularExpressions;

namespace calclog.utils.StringUtils
{
    public static class Utilities
    {
        public static string Filter(this string expression, string pattern, string replacement)
            => Regex.Replace(expression, pattern, replacement);

        public static Premise[] GetPremises(this string expression)
            => expression.Filter("[^a-z]", string.Empty).Select(premise => new Premise() { name = premise, value = null }).ToArray();

        public static bool GenerateTruthTable(string expression)
        {
            var premises = expression.GetPremises();
            var numberOfPremises = premises.Length;
        }
    }
}