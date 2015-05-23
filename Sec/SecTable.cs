using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sec
{
    public class SecTable
    {
        internal readonly Dictionary<string, SecToken> Data;
        internal readonly List<string> KeylessData; 
        public string Name { get; }

        public SecTable(string name)
        {
            Name = name;
            Data = new Dictionary<string, SecToken>();
            KeylessData = new List<string>();
        }

        public SecTable(string name, Dictionary<string, SecToken> tokens) : this(name)
        {
            Data = tokens;
        }

        public SecTable(string name, Dictionary<string, SecToken> tokens, List<string> keyless) : this(name, tokens)
        {
            KeylessData = keyless;
        }

        public SecToken this[string subtable] => Data[subtable];

        public TObject Get<TObject>(string name) where TObject : SecToken
        {
            var data = Data[name];
            if (data is TObject) return (TObject) data;
            throw new InvalidCastException("Data found within table does not match parameter type");
        }

        public string[] GetStrings()
        {
            return KeylessData.ToArray();
        }

        public override string ToString()
        {
            var b = new StringBuilder();
            b.AppendLine(Name);
            b.Append("Keyless: ");
            foreach (var k in KeylessData.Where(k => !string.IsNullOrWhiteSpace(k))) b.Append($"{k}, ");
            b.AppendLine();
            b.AppendLine("Data:");
            foreach (var kvp in Data)
            {
                b.AppendLine($"{kvp.Key} = {kvp.Value}");
            }
            return b.ToString();
        }
    }
}
