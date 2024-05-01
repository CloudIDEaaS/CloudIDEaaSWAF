using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public class ConditionPointsAttribute : Attribute
    {
        public Type ConditionPointsClass { get; }

        public ConditionPointsAttribute(Type conditionPointsClass)
        {
            this.ConditionPointsClass = conditionPointsClass;
        }
    }
}
