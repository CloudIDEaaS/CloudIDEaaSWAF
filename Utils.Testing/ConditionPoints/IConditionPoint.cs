using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils.ConditionPoints
{
    public interface IConditionPoint
    {
        List<IList<IConditionItem>> GetConditionLists();
    }
}