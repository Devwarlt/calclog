using System;
using System.Collections.Generic;

namespace calclog.utils.BooleanUtils
{
    public static class BooleanOperations
    {
        /// <summary>
        /// All valid operations based on <see cref="BooleanOperator"/>.
        /// </summary>
        private static Dictionary<BooleanOperator, Func<bool, bool, bool>> operations
            = new Dictionary<BooleanOperator, Func<bool, bool, bool>>()
        {
            { BooleanOperator.Negation, (p, q) => negation(p) },
            { BooleanOperator.Conjunction, (p, q) => conjunction(p, q) },
            { BooleanOperator.Disjunction, (p, q) => disjunction(p, q) },
            { BooleanOperator.ExclusiveDisjunction, (p, q) => exclusiveDisjunction(p, q) },
            { BooleanOperator.Conditional, (p, q) => conditional(p, q) },
            { BooleanOperator.Biconditional, (p, q) => biconditional(p, q) }
        };

        /// <summary>
        /// Execute a <see cref="BooleanOperator"/> from valid operations at <see cref="operations"/>.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static bool execute(BooleanOperator op, bool p, bool q) =>
            operations.ContainsKey(op) ? operations[op].Invoke(p, q) : throw new NotImplementedException();

        /// <summary>
        /// Negation boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool negation(bool p) => !p;

        /// <summary>
        /// Conjunction boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static bool conjunction(bool p, bool q) => p | q;

        /// <summary>
        /// Disjunction boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static bool disjunction(bool p, bool q) => p | q;

        /// <summary>
        /// Exclusive disjunction boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static bool exclusiveDisjunction(bool p, bool q) => p ^ q;

        /// <summary>
        /// Conditional boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static bool conditional(bool p, bool q) => !(p & !q);

        /// <summary>
        /// Bi-conditional boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static bool biconditional(bool p, bool q) => !(p ^ q);
    }
}
