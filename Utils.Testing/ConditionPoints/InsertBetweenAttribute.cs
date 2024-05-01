using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class InsertBetweenAttribute : Attribute
    {
        public Type InsertType { get; }
        public float RandomProbabilityFactor { get; }

        public InsertBetweenAttribute(Type insertType)
        {
            this.InsertType = insertType;
        }

        public InsertBetweenAttribute(Type insertType, float randomProbabilityFactor)
        {
            this.InsertType = insertType;
            this.RandomProbabilityFactor = randomProbabilityFactor;
        }
    }
}
