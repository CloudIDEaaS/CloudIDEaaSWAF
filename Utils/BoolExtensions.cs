using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class BoolExtensions
    {
        public static bool IsValueTrue(this bool? b)
        {
            return b.HasValue && b.Value;
        }
        public static bool IsValueFalse(this bool? b)
        {
            return b.HasValue && !b.Value;
        }
        public static bool AnyAreValueTrue(params bool?[] b)
        {
            foreach (var bValue in b)
            {
                if (bValue.HasValue && bValue.Value)
                {
                    return true;
                }
            }

            return false;
        }
        public static bool AnyAreValueFalse(params bool?[] b)
        {
            foreach (var bValue in b)
            {
                if (bValue.HasValue && !bValue.Value)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
