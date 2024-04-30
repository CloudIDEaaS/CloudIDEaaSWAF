using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.RulesEngine
{
    public class IsClause : Clause
    {
        public IsClause(string variable, string value) : base(variable, value)
        {
            Condition = "=";
        }

        protected override IntersectionType Intersect(Clause rhs)
        {
            if (rhs is IsClause)
            {
                if (value==rhs.Value)
                {
                    return IntersectionType.Include;
                }
                else
                {
                    return IntersectionType.MutuallyExclude;
                }
            }

            string v1 = value;
            string v2 = rhs.Value;

            double a = 0;
            double b = 0;

            if (double.TryParse(v1, out a) && double.TryParse(v2, out b))
            {
                if (rhs is LessClause)
                {
                    if (a >= b)
                    {
                        return IntersectionType.MutuallyExclude;
                    }
                    else
                    {
                        return IntersectionType.Include;
                    }
                }
                else if (rhs is LEClause)
                {
                    if (a > b)
                    {
                        return IntersectionType.MutuallyExclude;
                    }
                    else
                    {
                        return IntersectionType.Include;
                    }
                }
                else if (rhs is GreaterClause)
                {
                    if (a <= b)
                    {
                        return IntersectionType.MutuallyExclude;
                    }
                    else
                    {
                        return IntersectionType.Include;
                    }
                }
                else if (rhs is GEClause)
                {
                    if (a < b)
                    {
                        return IntersectionType.MutuallyExclude;
                    }
                    else
                    {
                        return IntersectionType.Include;
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
