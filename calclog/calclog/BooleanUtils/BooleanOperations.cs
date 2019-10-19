using calclog.ShuntingYardAlgorithm;
using System;
using System.Collections.Generic;

namespace calclog.BooleanUtils
{
    using bi = BooleanItem;

    /// <summary>
    /// Represents an utility class for boolean known operations.
    /// </summary>
    public static class BooleanOperations
    {
        /// <summary>
        /// All valid operations based on <see cref="bi"/>.
        /// </summary>
        public static Dictionary<bi, Func<bool, bool, bool>> operations
            = new Dictionary<bi, Func<bool, bool, bool>>()
        {
            { bi.NegationOperator, (p, q) => negation(p) },
            { bi.ConjunctionOperator, (p, q) => conjunction(p, q) },
            { bi.DisjunctionOperator, (p, q) => disjunction(p, q) },
            { bi.ExclusiveDisjunctionOperator, (p, q) => exclusiveDisjunction(p, q) },
            { bi.ConditionalOperator, (p, q) => conditional(p, q) },
            { bi.BiconditionalOperator, (p, q) => biconditional(p, q) }
        };

        /// <summary>
        /// Bi-conditional boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static bool biconditional(bool p, bool q) => !(p ^ q);

        /// <summary>
        /// Conditional boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static bool conditional(bool p, bool q) => !(p & !q);

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
        /// Negation boolean operation.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool negation(bool p) => !p;
    }
}
