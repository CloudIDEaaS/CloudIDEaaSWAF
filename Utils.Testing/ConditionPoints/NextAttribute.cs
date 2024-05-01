using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public enum NextKind
    {
        Invalid = -1,
        NotApplicable = 0,
        GoToOrderNumber,
        PickRandom,
        Terminates
    }

    public class NextAttribute : Attribute
    {
        public int OrderNumber { get; }
        public NextKind NextKind { get; }

        public NextAttribute(int orderNumber)
        {
            this.OrderNumber = orderNumber;
            this.NextKind = NextKind.GoToOrderNumber;
        }

        public NextAttribute(NextKind nextKind)
        {
            this.NextKind = nextKind;
        }
    }
}
