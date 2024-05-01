using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Utils.ConditionPoints
{
    public class ExpectedResponseAttribute : Attribute
    {
        public string ExpectedResponsePattern { get; }
        public int MinimumResponseBytes { get; }
        public TimeSpan MaximumResponseTimeSpan { get; }

        public ExpectedResponseAttribute(string expectedResponsePattern, int minimumResponseBytes = 0)
        {
            this.ExpectedResponsePattern = expectedResponsePattern;
            this.MinimumResponseBytes = minimumResponseBytes;
        }

        public ExpectedResponseAttribute(string expectedResponsePattern, string maximumResponseTimeSpan, int minimumResponseBytes = 0) : this(expectedResponsePattern, minimumResponseBytes)
        {
            this.MaximumResponseTimeSpan = TimeSpan.Parse(maximumResponseTimeSpan);
        }
    }
}
