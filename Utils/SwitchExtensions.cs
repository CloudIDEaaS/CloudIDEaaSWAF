using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils
{
    public interface ICaseObject
    {
        object CaseCondition { get; }
        void CallAction(object input);
        string StackTrace { get; set; }
    }

    internal class CaseObject : ICaseObject
    {
        public Action Action { get; private set; }
        public string StackTrace { get; set; }
        public object CaseCondition { get; private set; }

        [DebuggerHidden()]
        public CaseObject(object caseCondition, Action action)
        {
            this.CaseCondition = caseCondition;
            this.Action = action;
        }

        [DebuggerHidden()]
        public void CallAction(object input)
        {
            Action();
        }
    }

    internal class CaseElseObject : ICaseObject
    {
        public Action Action { get; private set; }
        public string StackTrace { get; set; }

        [DebuggerHidden()]
        public CaseElseObject(Action action)
        {
            this.Action = action;
        }

        [DebuggerHidden()]
        public void CallAction(object input)
        {
            Action();
        }

        public object CaseCondition
        {
            get { throw new NotImplementedException(); }
        }
    }

    internal class CaseAnyObject : ICaseObject
    {
        public Action Action { get; private set; }
        public object[] MultipleConditions { get; private set; }
        public string StackTrace { get; set; }

        [DebuggerHidden()]
        public CaseAnyObject(Action action, object[] multipleConditions)
        {
            this.Action = action;
            this.MultipleConditions = multipleConditions;
        }

        [DebuggerHidden()]
        public void CallAction(object input)
        {
            Action();
        }

        public object CaseCondition
        {
            get { throw new NotImplementedException(); }
        }
    }

    internal class TypedCaseObject<T> : ICaseObject
    {
        public object CaseCondition { get; private set; }
        public Action<T> Action { get; private set; }
        public string StackTrace { get; set; }

        [DebuggerHidden()]
        public TypedCaseObject(object caseCondition, Action<T> action)
        {
            this.CaseCondition = caseCondition;
            this.Action = action;
        }

        [DebuggerHidden()]
        public void CallAction(object input)
        {
            if (input is T)
            {
                Action((T)input);
            }
            else
            {
                var exception = new InvalidCastException(string.Format("Invalid Cast to {0} from {1}. Input = {2}, Case condition = {3}, StackTrace: \r\n{4}", typeof(T).FullName, input.GetType().FullName, input.ToString(), CaseCondition.ToString(), StackTrace));

                throw exception;
            }
        }
    }

    public class SwitchExtensions
    {
        [DebuggerHidden()]
        public static void Switch(object input, Func<object> switchItemFunc, params ICaseObject[] cases)
        {
            var item = switchItemFunc();

            foreach (var _case in cases)
            {
                if (_case is CaseElseObject)
                {
                    _case.CallAction(null);
                }
                else if (_case is CaseObject)
                {
                    if (_case.CaseCondition.Equals(item))
                    {
                        _case.CallAction(null);
                        break;
                    }
                }
                else if (_case is CaseAnyObject)
                {
                    if (item.IsOneOf(((CaseAnyObject)_case).MultipleConditions))
                    {
                        _case.CallAction(null);
                        break;
                    }
                }
                else if (_case.CaseCondition.Equals(item))
                {
                    _case.CallAction(input);
                    break;
                }
            }
        }

        [DebuggerHidden()]
        public static ICaseObject Case<T>(object caseCondition, Action<T> action)
        {
            var caseObject = new TypedCaseObject<T>(caseCondition, action);
            //var callStack = caseObject.GetStack(3).Skip(2).ToDelimitedList(string.Empty);

            //caseObject.StackTrace = callStack;

            return caseObject;
        }

        [DebuggerHidden()]
        public static ICaseObject Case(object caseCondition, Action action)
        {
            var caseObject = new CaseObject(caseCondition, action);
            //var callStack = caseObject.GetStack(3).Skip(2).ToDelimitedList(string.Empty);

            //caseObject.StackTrace = callStack;

            return caseObject;
        }

        [DebuggerHidden()]
        public static ICaseObject CaseAny(Action action, params object[] anyOf)
        {
            var caseObject = new CaseAnyObject(action, anyOf);
            //var callStack = caseObject.GetStack(3).Skip(2).ToDelimitedList(string.Empty);

            //caseObject.StackTrace = callStack;

            return caseObject;
        }

        [DebuggerHidden()]
        public static ICaseObject CaseElse(Action action)
        {
            var caseObject = new CaseElseObject(action);
            //var callStack = caseObject.GetStack(3).Skip(2).ToDelimitedList(string.Empty);

            //caseObject.StackTrace = callStack;

            return caseObject;
        }
    }
}
