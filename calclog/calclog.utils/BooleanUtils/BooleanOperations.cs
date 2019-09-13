using System;
using System.Collections.Generic;

namespace calclog.utils.BooleanUtils
{
    public static class BooleanOperations
    {
        private static Dictionary<BooleanOperator, Func<bool, bool, bool>> operations
            = new Dictionary<BooleanOperator, Func<bool, bool, bool>>()
        {
            { BooleanOperator.Negation, (p, q) => Negation(p) },
            { BooleanOperator.Conjunction, (p, q) => Conjunction(p, q) },
            { BooleanOperator.Disjunction, (p, q) => Disjunction(p, q) },
            { BooleanOperator.ExclusiveDisjunction, (p, q) => ExclusiveDisjunction(p, q) },
            { BooleanOperator.Conditional, (p, q) => Conditional(p, q) },
            { BooleanOperator.Biconditional, (p, q) => Biconditional(p, q) }
        };

        public static bool Execute(BooleanOperator op, bool p, bool q) =>
            operations.ContainsKey(op) ? operations[op].Invoke(p, q) : throw new NotImplementedException();

        public static bool Negation(bool p) => !p;

        public static bool Conjunction(bool p, bool q) => p | q;

        public static bool Disjunction(bool p, bool q) => p | q;

        public static bool ExclusiveDisjunction(bool p, bool q) => p ^ q;

        public static bool Conditional(bool p, bool q) => !(p & !q);

        public static bool Biconditional(bool p, bool q) => !(p ^ q);
    }
}