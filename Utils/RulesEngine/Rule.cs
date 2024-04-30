using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.RulesEngine
{
    public class Rule
    {
        protected Clause consequent;
        protected List<Clause> antecedents;
        protected bool fired;
        protected string name;
        protected int index = 0;

        public Rule(string name)
        {
            antecedents = new List<Clause>();
            this.name = name;
            fired = false;
        }

        public void FirstAntecedent()
        {
            index = 0;
        }

        public bool HasNextAntecedents()
        {
            return index < antecedents.Count;
        }

        public Clause NextAntecedent()
        {
            Clause c = antecedents[index];
            index++;
            return c;
        }

        public string getName()
        {
            return name;
        }

        public void setConsequent(Clause consequent)
        {
            this.consequent = consequent;
        }

        public void AddAntecedent(Clause antecedent)
        {
            antecedents.Add(antecedent);
        }

        public Clause getConsequent()
        {
            return consequent;
        }

        public bool isFired()
        {
            return fired;
        }

        public void fire(WorkingMemory wm)
        {
            if (!wm.IsFact(consequent))
            {
                wm.AddFact(consequent);
            }

            fired = true;
        }

        public bool isTriggered(WorkingMemory wm)
        {
            foreach (Clause antecedent in antecedents) 
            {
                if (!wm.IsFact(antecedent))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
