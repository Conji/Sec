namespace Sec
{
    public class SecInt : SecToken
    {
        public int Value { get; set; }
        public override SecType Type => SecType.Int;

        public SecInt(int value)
        {
            Value = value;
        }

        public static implicit operator int(SecInt left)
        {
            return left.Value;
        }

        public static SecInt operator +(SecInt left, SecInt right)
        {
            return new SecInt(left.Value + right.Value);
        }

        public static SecInt operator -(SecInt left, SecInt right)
        {
            return new SecInt(left.Value - right.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
