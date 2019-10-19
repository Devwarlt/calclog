namespace calclog.ShuntingYardAlgorithm
{
    /// <summary>
    /// Represents an enumerator of items valid along
    /// <see cref="BooleanSymbol.expressionSymbols"/> dictionary.
    /// </summary>
    public enum BooleanItem
    {
        /// <summary>
        /// Represents '1' symbol on expression.
        /// </summary>
        True,

        /// <summary>
        /// Represents '0' symbol on expression.
        /// </summary>
        False,

        /// <summary>
        /// Represents '(' symbol on expression.
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// Represents ')' symbol on expression.
        /// </summary>
        RightParenthesis,

        /// <summary>
        /// Represents a negation operator conective on expression.
        /// </summary>
        NegationOperator,

        /// <summary>
        /// Represents a biconditional operator conective on expression.
        /// </summary>
        BiconditionalOperator,

        /// <summary>
        /// Represents a conditional operator conective on expression.
        /// </summary>
        ConditionalOperator,

        /// <summary>
        /// Represents an exclusive disjunction operator conective on expression.
        /// </summary>
        ExclusiveDisjunctionOperator,

        /// <summary>
        /// Represents a conjunction operator conective on expression.
        /// </summary>
        ConjunctionOperator,

        /// <summary>
        /// Represents a disjunction operator conective on expression.
        /// </summary>
        DisjunctionOperator
    }
}
