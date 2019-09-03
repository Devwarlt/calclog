namespace calclog.utils.BooleanOperations
{
    public static partial class BooleanOperator
    {
        public struct Biconditional : IBooleanOperator
        {
            public bool p;
            public bool q;

            public Biconditional(bool p, bool q)
            {
                this.p = p;
                this.q = q;
            }

            public bool Get() => !(p ^ q);
        }
    }
}