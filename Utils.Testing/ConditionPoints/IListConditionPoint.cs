using System.Collections.Generic;

namespace Utils.ConditionPoints
{
    public interface IListConditionPoint
    {
        List<IConditionItem> GenerateConditionItems();
    }
}