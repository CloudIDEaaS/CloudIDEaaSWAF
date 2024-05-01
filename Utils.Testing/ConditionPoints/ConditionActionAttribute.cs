using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public class ConditionActionAttribute : Attribute
    {
        public Type ActionPointType { get; }

        public ConditionActionAttribute(Type actionPointType)
        {
            this.ActionPointType = actionPointType;
        }
    }
}
