namespace Sec
{
    public class SecBool : SecToken
    {
        public bool Value { get; set; }
        public override SecType Type => SecType.Bool;

        public SecBool(bool value)
        {
            Value = value;
        }

        public static implicit operator bool(SecBool b)
        {
            return b.Value; 
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
