using System;

namespace calclog.utils.BooleanUtils
{
    public abstract class Proposition
    {
        public BooleanOperators op;

        public abstract bool Result();
    }

    public sealed class SimpleProposition : Proposition
    {
        public Premise premise;

        public override bool Result() => !premise.value.Value;
    }

    public sealed class ComplexProposition : Proposition
    {
        public Premise[] premises;
        public Proposition[] propositions;

        public override bool Result()
            => propositions.Length != 0 ? ;
    }
}