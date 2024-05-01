using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public delegate void CreateOnDemandEventHandler(object sender, CreateOnDemandEventArgs e);

    public class CreateOnDemandEventArgs
    {
        public IOnDemandConditionList List { get; }
        public Action<IConditionItem> AddItem { get; }
        public CreateOnDemandEventArgs(IOnDemandConditionList onDemandConditionList, Action<IConditionItem> addItem)
        {
            this.List = onDemandConditionList;
            this.AddItem = addItem;
        }
    }
}
