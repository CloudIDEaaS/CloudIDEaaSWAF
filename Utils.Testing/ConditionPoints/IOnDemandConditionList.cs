using System;
using System.Collections.Generic;

namespace Utils.ConditionPoints
{
    public interface IOnDemandConditionList : ICloneable
    {
        event CreateOnDemandEventHandler CreateOnDemand;
        IEnumerable<IConditionItem> Group { get; set; }
    }
}