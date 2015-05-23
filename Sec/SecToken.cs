using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sec
{
    public abstract class SecToken
    {
        public abstract SecType Type { get; }
    }

    public enum SecType
    {
        String,
        Int,
        Float,
        Bool,
        Keyless
    }
}
