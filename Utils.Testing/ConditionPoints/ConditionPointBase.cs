using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public abstract class ConditionPointBase : IConditionPoint
    {
        public ConditionPointKind ConditionPointType { get; }
        public List<ConditionPointDependency> ConditionPointDependencies { get; }
        public IServiceProvider ServiceProvider { get; set; }

        public abstract List<IList<IConditionItem>> GetConditionLists();

        public ConditionPointBase(ConditionPointKind conditionPointKind)
        {
            this.ConditionPointType = conditionPointKind;
            this.ConditionPointDependencies = new List<ConditionPointDependency>();
        }
    }
}
