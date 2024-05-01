using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ConditionPoints
{
    public enum HandlerResult
    {
        NotExpected = -1,
        Continue,
        Terminate
    }

    public interface IStepHandler
    {
        public ISessionStateObject SessionState { get; set; }
        public HandlerResult Handle(string phase, IEnumerable<IConditionItem> conditionItems, IConditionItem conditionItem, params KeyValuePair<string, object>[] parms);
        public HandlerResult Handle(params KeyValuePair<string, object>[] parms);
    }
}
