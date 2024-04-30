using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.RulesEngine
{
    public class Clause
    {
        protected string variable;
        protected string value;
        public string Condition { get; protected set; } = "=";

        public Clause(string variable, string value)
        {
            this.variable = variable;
            this.value = value;
        }

        public Clause(string variable, string condition, string value)
        {
            this.variable = variable;
            this.value = value;
            Condition = condition;
        }

        public String Variable
        {
            get { return variable; }
        }

        public string Value
        {
            get { return value; }
        }

        public IntersectionType MatchClause(Clause rhs)
        {
            if (variable!=rhs.Variable)
            {
                return IntersectionType.Unknown;
            }

            return Intersect(rhs);
        }

        protected virtual IntersectionType Intersect(Clause rhs)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return variable + " " + Condition + " " + value;
        }
    }
}
