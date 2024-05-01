using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public class OnDemandConditionItem<T> : ConditionItem<T>, IOnDemandConditionItem
    {
        public OnDemandConditionItem(string name, T item) : base(name, item)
        {
        }
    
        public IEnumerable<IConditionItem> Group 
        { 
            get
            {
                return ((IOnDemandConditionList)this.Item).Group;
            }

            set
            {
                ((IOnDemandConditionList)this.Item).Group = value;
            }
        }


        public object Clone()
        {
            T item;

            if (this.Item is ICloneable)
            {
                item = (T)((ICloneable)this.Item).Clone();
            }
            else
            {
                item = this.Item;
            }

            return new OnDemandConditionItem<T>(this.Name, item)
            {
                ActionPointType = this.ActionPointType,
                Weight = this.Weight,
                OrderNumber = this.OrderNumber,
                NextOrderNumber = this.NextOrderNumber,
                OrderKind = this.OrderKind,
                NextKind = this.NextKind,
                NextItem = this.NextItem,
                Group = this.Group
            };
        }
    }
}
