using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class e
    {
        public static void AssertInRange<T>(this T value, T lowerLimit, T upperLimit) where T : IComparable
        {
            if (!value.IsBetween(lowerLimit, upperLimit))
            {
                e.Throw<ArgumentOutOfRangeException>("Value {0} not in valid range of {1} and {2}", value, lowerLimit, upperLimit);
            }
        }

        public static object Throw<ExceptionType>(this string format, params object[] args)
        {
            var message = string.Format(format, args);

            Throw<ExceptionType>(message);

            return null;
        }

        [DebuggerStepThrough()]
        public static object Throw<ExceptionType>(this string message)
        {
            var type = typeof(ExceptionType);
            Exception exception = null;

            if (type.Implements<Exception>())
            {
                try
                {
                    var constructor = type.GetConstructor(new Type[] { typeof(string) });
                    
                    exception = (Exception)constructor.Invoke(new object[] { message });
                }
                catch (Exception ex)
                {
                    throw new NotImplementedException("e.Throw cannot process Exception type '{0}'. Does not have contructor that takes single argument string".AsFormat(typeof(ExceptionType)));
                }

                if (exception != null)
                {
                    throw exception;
                }
            }

            return null;
        }
    }

    public static class ExceptionExtensions
    {
        public static string GetExtendedMessage(this Exception ex)
        {
            if (ex.InnerException != null)
            {
                return string.Format("Exception:{0}, InnerException:{1}", ex.Message, ex.InnerException.Message);
            }
            else
            {
                return ex.Message;
            }
        }
    }
}
