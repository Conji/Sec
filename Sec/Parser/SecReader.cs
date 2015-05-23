using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sec.Parser
{
    internal static class SecReader
    {
        public static Dictionary<string, SecTable> GetTables(string[] contents)
        {
            foreach (var nc in contents.Where(l => l.StartsWith("import ")).Select(line => File.ReadAllLines(GetImport(line))))
            {
                Array.Copy(nc, 0, contents, contents.Length, nc.Length);
            }

            return contents.Where(line => line.StartsWith("("))
                .Select(GetTableName)
                .ToDictionary(name => name, name => ReadTable(name, contents));
        }

        public static string GetImport(string line)
        {
            return line
                .Replace("import", "")
                .Replace("'", "")
                .Replace("\"", "");
        }

        public static SecTable ReadTable(string name, string[] contents)
        {
            var reading = false;
            var table = new SecTable(name);
            if (name.Contains("."))
            {
                table = ReadTable(GetParentName(name), contents);
            }
            foreach (var c in contents.Where(c => !c.StartsWith("--") || !c.StartsWith("#")))
            {
                if (!reading)
                {
                    if (c.StartsWith($"({name})"))
                    {
                        reading = true;
                        if (c.Contains(">"))
                        {
                            foreach (var inht in GetInherits(c).Select(inh => ReadTable(inh, contents)))
                            {
                                table.KeylessData.AddRange(inht.KeylessData);
                                foreach (var kvp in inht.Data)
                                {
                                    table.Data.Add(kvp.Key, kvp.Value);
                                }
                            }
                        }
                    }
                    continue;
                }
                if (c.StartsWith("(")) break;
                var type = GetType(c);
                var kn = GetKeyName(c);
                if (table.Data.ContainsKey(kn)) table.Data.Remove(kn);
                switch (type)
                {
                    case SecType.Bool:
                        table.Data.Add(kn, new SecBool(bool.Parse(c.Split('=')[1])));
                        break;
                    case SecType.Float:
                        table.Data.Add(kn, new SecFloat(float.Parse(c.Split('=')[1])));
                        break;
                    case SecType.Int:
                        table.Data.Add(kn, new SecInt(int.Parse(c.Split('=')[1])));
                        break;
                    case SecType.Keyless:
                        table.KeylessData.Add(c.Trim());
                        break;
                    case SecType.String:
                        table.Data.Add(kn, new SecString(c.Split('=')[1].Trim()));
                        break;
                }

            }
            return table;
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
            return line.Replace("(", "").Replace(")", "").Split('>')[0].Trim();
        }

        public static string GetParentName(string name)
        {
            if (!name.Contains(".")) throw new ArgumentOutOfRangeException(nameof(name), $"{name} cannot have a parent table");
            if (name[0] == '.' || name.Last() == '.') throw new ArgumentException("Cannot start or end table name with '.'", nameof(name));
            var i = name.LastIndexOf('.');
            return name.Remove(i);
        }

        public static string[] GetInherits(string line)
        {
            return line.Split('>')[1].Split(',');
        }

        public static string GetKeyName(string line)
        {
            return line.Split('=')[0].Trim();
        }
    }
}
