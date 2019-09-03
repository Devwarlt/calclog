namespace calclog.utils.BooleanOperations
{
    public static partial class BooleanOperator
    {
        public struct ExclusiveDisjunction : IBooleanOperator
        {
            public bool p;
            public bool q;

            public ExclusiveDisjunction(bool p, bool q)
            {
                this.p = p;
                this.q = q;
            }

            public bool Get() => p ^ q;
        }
    }
}