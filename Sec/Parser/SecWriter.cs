using System.Collections.Generic;
using System.Linq;

namespace Sec.Parser
{
    internal static class SecWriter
    {
        public static string[] Convert(SecFile sec)
        {
            var l = new List<string>();
            foreach (var table in sec.Tables)
            {
                l.Add($"({table.Key})");
                l.AddRange(table.Value.KeylessData.Where(kd => !string.IsNullOrWhiteSpace(kd)));
                l.AddRange(table.Value.Data.Select(kvp => $"{kvp.Key} = {kvp.Value}"));
            }
            return l.ToArray();
        }
    }
}
