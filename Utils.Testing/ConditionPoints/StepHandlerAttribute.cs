using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public class StepHandlerAttribute : Attribute
    {
        public Type HandlerType { get; }
        public string ExpectedResponsePattern { get; }

        public StepHandlerAttribute(Type handlerType, string expectedResponsePattern = null)
        {
            this.HandlerType = handlerType;
            this.ExpectedResponsePattern = expectedResponsePattern;
        }
    }
}
