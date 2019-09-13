namespace calclog.utils.BooleanUtils
{
    public class Proposition
    {
        public PropositionFlag flag = PropositionFlag.Empty;
        public BooleanOperator op;
        public char[] premises;

        public bool Result(bool p, bool q) => BooleanOperations.Execute(op, p, q);
    }
}