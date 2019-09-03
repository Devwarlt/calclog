namespace calclog.utils.BooleanOperations
{
    public static partial class BooleanOperator
    {
        public struct Negation : IBooleanOperator
        {
            public bool p;

            public Negation(bool p) => this.p = p;

            public bool Get() => !p;
        }
    }
}