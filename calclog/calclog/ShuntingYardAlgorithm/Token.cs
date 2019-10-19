namespace calclog.ShuntingYardAlgorithm
{
    /// <summary>
    /// Represents flags used along deserialization procedure (<see cref="BooleanExpression.insertMissingConjunctions"/>)
    /// and expression symbol structured format (<see cref="BooleanSymbol.expressionSymbols"/>).
    /// </summary>
    public enum Token
    {
        /// <summary>
        /// Represents a default flag.
        /// </summary>
        Unknown,

        /// <summary>
        /// Represents a premise flag.
        /// </summary>
        Premise,

        /// <summary>
        /// Represents an operator flag.
        /// </summary>
        Operator,

        /// <summary>
        /// Represents '(' symbol flag.
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// Represents ')' symbol flag.
        /// </summary>
        RightParenthesis
    }
}
