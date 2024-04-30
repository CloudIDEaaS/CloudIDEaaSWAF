using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.RulesEngine
{
    public class LEClause : Clause
    {
        public LEClause(string variable, string value) : base(variable, value)
        {
            Condition = "<=";
        }

        protected override IntersectionType Intersect(Clause rhs)
        {
            string v1 = value;
            string v2 = rhs.Value;

            double a = 0;
            double b = 0;

            if (double.TryParse(v1, out a) && double.TryParse(v2, out b))
            {
                if (rhs is LEClause)
                {
                    //v1 <= a
                    //v2 <= b 
                    //matched: b <= a
                    //unmatched: b > a
                    if (b <= a)
                    {
                        return IntersectionType.Include;
                    }
                    else
                    {
                        return IntersectionType.Unknown;
                    }
                }
                else if (rhs is LessClause)
                {
                    //v1 <= a
                    //v2 < b 
                    //matched: b <= a
                    //unmatched: b > a
                    if (b <= a)
                    {
                        return IntersectionType.Include;
                    }
                    else
                    {
                        return IntersectionType.Unknown;
                    }
                }
                else if (rhs is IsClause)
                {
                    //v1 <= a
                    //v2 = b
                    //matched: b <= a
                    //mutually exclusive: b > a
                    if (b <= a)
                    {
                        return IntersectionType.Include;
                    }
                    else
                    {
                        return IntersectionType.MutuallyExclude;
                    }
                }
                else if (rhs is GEClause)
                {
                    //v1 <= a
                    //v2 >= b
                    //mutually exclusive: b > a
                    //unmatched: b <= a
                    if (b > a)
                    {
                        return IntersectionType.MutuallyExclude;
                    }
                    else
                    {
                        return IntersectionType.Unknown;
                    }
                }
                else if (rhs is GreaterClause)
                {
                    //v1 <= a
                    //v2 > b
                    //mutually exclusive: b >= a
                    //unmatched: b < a
                    if (b >= a)
                    {
                        return IntersectionType.MutuallyExclude;
                    }
                    else
                    {
                        return IntersectionType.Unknown;
                    }
                }
                else
                {
                    return IntersectionType.Unknown;
                }
            }
            else
            {
                return IntersectionType.Unknown;
            }
        }
    }
}
