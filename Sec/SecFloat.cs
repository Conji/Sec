namespace Sec
{
    public class SecFloat : SecToken
    {
        public override SecType Type => SecType.Float;
        public float Value { get; set; }

        public SecFloat(float value)
        {
            Value = value;
        }

        public static implicit operator float(SecFloat f)
        {
            return f.Value;
        }

        public override string ToString()
        {
            return Value.ToString("f");
        }
    }
}
