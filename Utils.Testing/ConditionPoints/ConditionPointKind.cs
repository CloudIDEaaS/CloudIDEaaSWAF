using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public enum ConditionPointKind
    {
        FactoringList = 1,
        NonFactoringList = 1 << 1,
        RandomList = 1 << 2 | FactoringList,
        RandomListSource = 1 << 3 | NonFactoringList,
    }
}
