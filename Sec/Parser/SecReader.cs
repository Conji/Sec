using System;
using System.Collections.Generic;
using System.Linq;

namespace Sec.Parser
{
    internal static class SecReader
    {
        public static Dictionary<string, SecTable> GetTables(string[] contents)
        {
            var table = new Dictionary<string, SecTable>();
            var n = "";
            foreach (var line in contents.Where(line => !line.StartsWith("--")))
            {
                if (line.StartsWith("import "))
                {
                    var sec = SecFile.Open(GetImport(line)).Tables;
                    foreach (var s in sec)
                    {
                        table.Add(s.Key, s.Value);
                    }
                }
                else if (line.StartsWith("("))
                {
                    var name = GetTableName(line);
                    SecTable t;
                    if (name.Contains(".") && table.ContainsKey(GetParentName(name)))
                    {
                        t = table[GetParentName(name)];
                    }
                    else
                    {
                        t = new SecTable(name);
                    }
                    table.Add(name, t);
                    n = name;
                }
                else
                {
                    var type = GetType(line);
                    var kn = GetKeyName(line);
                    if (table[n].Data.ContainsKey(kn)) table[n].Data.Remove(kn);
                    switch (type)
                    {
                        case SecType.Bool:
                            table[n].Data.Add(kn, new SecBool(bool.Parse(line.Split('=')[1])));
                            break;
                        case SecType.Float:
                            table[n].Data.Add(kn, new SecFloat(float.Parse(line.Split('=')[1])));
                            break;
                        case SecType.Int:
                            table[n].Data.Add(kn, new SecInt(int.Parse(line.Split('=')[1])));
                            break;
                        case SecType.Keyless:
                            table[n].KeylessData.Add(line.Trim());
                            break;
                        case SecType.String:
                            table[n].Data.Add(kn, new SecString(line.Split('=')[1].Trim()));
                            break;
                    }
                }
            }

            return table;
        }

        public static string GetImport(string line)
        {
            return line
                .Replace("import", "")
                .Replace("'", "")
                .Replace("\"", "");
        }

        public static SecType GetType(string s)
        {
            if (!s.Contains("=")) return SecType.Keyless;
            var ns = s.Split('=')[1].Trim();
            if (ns == "true" || ns == "false") return SecType.Bool;
            
            int i;
            if (int.TryParse(ns, out i)) return SecType.Int;
            float f;
            if (float.TryParse(ns, out f)) return SecType.Float;

            return SecType.String;
        }

        public static string GetTableName(string line)
        {
            return line.Replace("(", "").Replace(")", "").Trim();
        }

        public static string GetParentName(string name)
        {
            if (!name.Contains(".")) throw new ArgumentOutOfRangeException(nameof(name), $"{name} cannot have a parent table");
            if (name[0] == '.' || name.Last() == '.') throw new ArgumentException("Cannot start or end table name with '.'", nameof(name));
            var i = name.LastIndexOf('.');
            return name.Remove(i);
        }

        public static string GetKeyName(string line)
        {
            return line.Split('=')[0].Trim();
        }
    }
}
