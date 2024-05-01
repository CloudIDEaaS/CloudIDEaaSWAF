using System;

namespace Utils.ConditionPoints
{
    public interface IComparableListConditionPoint : IListConditionPoint
    {
        object FanListSetting { get; }
        void FanList();
    }
}