namespace calclog.utils.BooleanOperations
{
    public static partial class BooleanOperator
    {
        public struct Conditional : IBooleanOperator
        {
            public bool p;
            public bool q;

            public Conditional(bool p, bool q)
            {
                this.p = p;
                this.q = q;
            }

            public bool Get() => !(p & !q);
        }
    }
}