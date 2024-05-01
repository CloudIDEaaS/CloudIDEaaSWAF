using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class WeightAttribute : Attribute
    {
        public float Weight { get; }

        public WeightAttribute(float weight)
        {
            this.Weight = weight;
        }
    }
}
