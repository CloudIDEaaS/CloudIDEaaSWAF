using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Utils.ConditionPoints
{
    [DebuggerDisplay(" { DebugInfo } ")]
    public class ConditionItem<T> : IConditionItem
    {
        public T Item { get; }
        public string Name { get; }
        public Type ActionPointType { get; set; }
        public string ExpectedResponsePattern { get; set; }
        public int MinimumResponseBytes { get; set; }
        public TimeSpan MaximumResponseTimeSpan { get; set; }
        object IConditionItem.Item => this.Item;
        public float Weight { get; set; }
        public int OrderNumber { get; set; }
        public int NextOrderNumber { get; set; }
        public OrderKind OrderKind { get; set; }
        public NextKind NextKind { get; set; }
        public IConditionItem NextItem { get; set; }
        public IStepHandler StepHandler { get; set; }

        public ConditionItem(string name, T item)
        {
            this.Item = item;
            this.Name = name;
        }

        public string DebugInfo
        {
            get
            {
                return this.Item.ToString();
            }
        }

    }
}
