using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public enum CountRelationshipKind
    {
        RandomScopedRange,          // picks a single count list within a range 
        OrderedCountCrossJoin,      // cross joins to count list starting from lowest to highest
        OrderedCountCrossJoinDesc,  // cross joins to count list starting from highest to lowest
        RandomCrossJoin             // cross joins to count list with numers randomized
    }

    public class RandomizationCountFromAttribute : Attribute
    {
        public Type CountType { get; }
        public CountRelationshipKind CountRelationshipKind { get; }

        public RandomizationCountFromAttribute(Type countType, CountRelationshipKind countRelationshipKind)
        {
            this.CountType = countType;
            this.CountRelationshipKind = countRelationshipKind;
        }
    }
}
