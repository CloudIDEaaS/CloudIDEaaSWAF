using System;

namespace Utils.ConditionPoints
{
    public interface IConditionItem
    {
        string Name { get; }
        Type ActionPointType { get; set; }
        string ExpectedResponsePattern { get; set; }
        int MinimumResponseBytes { get; set; }
        TimeSpan MaximumResponseTimeSpan { get; set; }
        object Item { get; }
        float Weight { get; set; }
        int OrderNumber { get; }
        int NextOrderNumber { get; }
        OrderKind OrderKind { get; set; }
        NextKind NextKind { get; set; }
        IConditionItem NextItem { get; set; }
        IStepHandler StepHandler { get; set; }
    }
}