namespace Sec
{
    public class SecString : SecToken
    {
        public string Value { get; set; }
        public override SecType Type => SecType.String;

        public SecString(string value)
        {
            Value = value;
        }

        public static implicit operator string(SecString s)
        {
            return s.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
