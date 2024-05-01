using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Class)]
    public class TerminationWeightAttribute : Attribute
    {
        public float Weight { get; }

        public TerminationWeightAttribute(float weight)
        {
            this.Weight = weight;
        }
    }
}
