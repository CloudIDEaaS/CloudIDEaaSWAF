using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public enum OrderKind
    {
        Invalid = -1,
        Unordered = 0,
        Specific,
        Randomized,
    }

    public class OrderAttribute : Attribute
    {
        public int Order { get; }
        public OrderKind OrderKind { get; }

        public OrderAttribute(int order)
        {
            this.Order = order;
            this.OrderKind = OrderKind.Specific;
        }

        public OrderAttribute(OrderKind orderKind)
        {
            this.OrderKind = orderKind;
        }
    }
}
