using System;
using System.Collections.Generic;

namespace Utils.ConditionPoints
{
    public interface IOnDemandConditionItem : ICloneable
    {
        IEnumerable<IConditionItem> Group { get; set; }
    }
}