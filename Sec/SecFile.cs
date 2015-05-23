using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sec.Parser;

namespace Sec
{
    public sealed class SecFile
    {
        private readonly string _location;
        private const string SEC_EXT = ".sec";
        internal Dictionary<string, SecTable> Tables; 

        public SecFile(string location)
        {
            _location = location + SEC_EXT;
            Tables = new Dictionary<string, SecTable>();
        }

        public SecFile() : this("config")
        {
            
        }

        public SecTable this[string tableName] => Tables[tableName];

        public static SecFile Open(string location)
        {
            return new SecFile(location) { Tables = SecReader.GetTables(File.ReadAllLines(location))};
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var line in SecWriter.Convert(this))
            {
                builder.AppendLine(line);
            }
            return builder.ToString();
        }
    }
}
