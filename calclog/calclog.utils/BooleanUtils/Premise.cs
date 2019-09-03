namespace calclog.utils.BooleanUtils
{
    public struct Premise
    {
        public char name;
        public bool? value;

        public Premise(char name, bool? value)
        {
            this.name = name;
            this.value = value;
        }
    }
}