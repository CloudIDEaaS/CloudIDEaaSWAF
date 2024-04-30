using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.RulesEngine
{
    public class WorkingMemory
    {
        protected List<Clause> facts;

        public WorkingMemory()
        {
            facts = new List<Clause>();
        }

        public void AddFact(Clause fact)
        {
            facts.Add(fact);
        }

        public bool IsNotFact(Clause c)
        {
            foreach(Clause fact in facts)
            {
                if (fact.MatchClause(c) == IntersectionType.MutuallyExclude)
                {
                    return true;
                }
            }

            return false;
        }

        public void ClearFacts()
        {
            facts.Clear();
        }

        public bool IsFact(Clause c)
        {
            foreach(Clause fact in facts)
            {
                if (fact.MatchClause(c) == IntersectionType.Include)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder message = new StringBuilder();

            bool first_clause = true;
            foreach(Clause cc in facts)
            {
                if (first_clause)
                {
                    message.Append(cc.ToString());
                    first_clause = false;
                }
                else
                {
                    message.Append("\n"+cc.ToString());
                }
            }

            return message.ToString();
        }

        public int Count
        {
            get { return facts.Count;  }
        }
    }
}
