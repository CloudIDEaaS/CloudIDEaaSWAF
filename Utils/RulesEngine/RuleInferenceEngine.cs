using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.RulesEngine
{
    public class RuleInferenceEngine
    {
        protected List<Rule> rules = new List<Rule>();
        protected WorkingMemory workingMemory = new WorkingMemory();

        public RuleInferenceEngine()
        {

        }

        public void AddRule(Rule rule)
        {
            rules.Add(rule);
        }

        public void ClearRules()
        {
            rules.Clear();
        }

        //forward chain
        public void Infer()
        {
            List<Rule> cs;

            do
            {
                cs = Match();

                if (cs.Count > 0)
                {
                    if (!FireRule(cs))
                    {
                        break;
                    }
                }
            } while (cs.Count > 0);
        }

        //backward chain
        public Clause Infer(string goal_variable, List<Clause> unproved_conditions)
        {
            Clause conclusion = null;
            var goal_stack = new List<Rule>();

            foreach(Rule rule in rules)
            {
                Clause consequent = rule.getConsequent();
                if (consequent.Variable==goal_variable)
                {
                    goal_stack.Add(rule);
                }
            }

            foreach(Rule rule in rules)
            {
                rule.FirstAntecedent();
                bool goalReached = false;

                while (rule.HasNextAntecedents())
                {
                    Clause antecedent = rule.NextAntecedent();
                    if (!workingMemory.IsFact(antecedent))
                    {
                        if (workingMemory.IsNotFact(antecedent)) //conflict with memory
                        {
                            goalReached = false;
                            break;
                        }
                        else if (IsFact(antecedent, unproved_conditions)) //deduce to be a fact
                        {
                            workingMemory.AddFact(antecedent);
                        }
                        else //deduce to not be a fact
                        {
                            goalReached = false;
                            break;
                        }
                    }
                }

                if (goalReached)
                {
                    conclusion = rule.getConsequent();
                    break;
                }
            }

            return conclusion;
        }

        public void ClearFacts()
        {
            workingMemory.ClearFacts();
        }

        protected bool IsFact(Clause goal, List<Clause> unproved_conditions)
        {
            List<Rule> goal_stack = new List<Rule>();

            foreach(Rule rule in rules)
            {
                Clause consequent = rule.getConsequent();
                IntersectionType it = consequent.MatchClause(goal);
                if (it == IntersectionType.Include)
                {
                    goal_stack.Add(rule);
                }
            }

            if (goal_stack.Count == 0)
            {
                unproved_conditions.Add(goal);
            }
            else
            {
                foreach(Rule rule in goal_stack)
                {
                    rule.FirstAntecedent();
                    bool goal_reached = true;
                    while (rule.HasNextAntecedents())
                    {
                        Clause antecedent = rule.NextAntecedent();
                        if (!workingMemory.IsFact(antecedent))
                        {
                            if (workingMemory.IsNotFact(antecedent))
                            {
                                goal_reached = false;
                                break;
                            }
                            else if (IsFact(antecedent, unproved_conditions))
                            {
                                workingMemory.AddFact(antecedent);
                            }
                            else
                            {
                                goal_reached = false;
                                break;
                            }
                        }
                    }

                    if (goal_reached)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected bool FireRule(List<Rule> conflictingRules)
        {
            bool hasRule2Fire = false;
            foreach(Rule rule in conflictingRules)
            {
                if (!rule.isFired())
                {
                    hasRule2Fire = true;
                    rule.fire(workingMemory);
                }
            }

            return hasRule2Fire;

        }

        /// <summary>
        /// property indicating the known facts in the current working memory
        /// </summary>
        public WorkingMemory Facts
        {
            get
            {
                return workingMemory;
            }
        }

        /// <summary>
        /// Add another know fact into the working memory
        /// </summary>
        /// <param name="c"></param>
        public void AddFact(Clause c)
        {
            workingMemory.AddFact(c);
        }

        /// <summary>
        /// Method that return the set of rules whose antecedents match with the working memory
        /// </summary>
        /// <returns></returns>
        protected List<Rule> Match()
        {
            List<Rule> cs = new List<Rule>();
            foreach(Rule rule in rules)
            {
                if (rule.isTriggered(workingMemory))
                {
                    cs.Add(rule);
                }
            }
            return cs;
        }
    }
}
