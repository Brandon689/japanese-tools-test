using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YomiChanPort
{
    public static class CompilerCharExtension
    {
        public static int CodePoint(this Char c)
        {
            return (int)c;
        }
    }
}
