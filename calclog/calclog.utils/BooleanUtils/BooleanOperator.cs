namespace calclog.utils.BooleanUtils
{
    public enum BooleanOperator
    {
        /// <summary>
        /// Represents a boolean operation flag for <see cref="BooleanOperations.negation(bool)"/> expression.
        /// </summary>
        Negation,

        /// <summary>
        /// Represents a boolean operation flag for <see cref="BooleanOperations.conjunction(bool, bool)"/> expression.
        /// </summary>
        Conjunction,

        /// <summary>
        /// Represents a boolean operation flag for <see cref="BooleanOperations.disjunction(bool, bool)"/> expression.
        /// </summary>
        Disjunction,

        /// <summary>
        /// Represents a boolean operation flag for <see cref="BooleanOperations.exclusiveDisjunction(bool, bool)"/> expression.
        /// </summary>
        ExclusiveDisjunction,

        /// <summary>
        /// Represents a boolean operation flag for <see cref="BooleanOperations.conditional(bool, bool)"/> expression.
        /// </summary>
        Conditional,

        /// <summary>
        /// Represents a boolean operation flag for <see cref="BooleanOperations.biconditional(bool, bool)"/> expression.
        /// </summary>
        Biconditional
    }
}
