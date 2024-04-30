using System;
using System.Net;

namespace Utils
{
    public delegate CompareWithContinuation CompareWithAdditionalComparer(CompareWithInfoBase compareWithInfo, CompareWithComparisonResult comparisonResult);

    public abstract class CompareWithInfoBase
    {
        public abstract Type CompareType { get; }
        public abstract CompareWithComparisonResult InitialResult { get; protected set;  }
        public abstract object OriginalValue1 { get; protected set; }
        public abstract object OriginalValue2 { get; protected set; }
    }

    public class CompareWithInfo<T> : CompareWithInfoBase
    {
        public T ProcessedValue1 { get; private set; }
        public T ProcessedValue2 { get; private set; }
        public override object OriginalValue1 { get; protected set; }
        public override object OriginalValue2 { get; protected set; }
        public override CompareWithComparisonResult InitialResult { get; protected set; }

        public CompareWithInfo(T processedValue1, T processedValue2, object originalValue1, object originalValue2, CompareWithComparisonResult result)
        {
            this.ProcessedValue1 = processedValue1;
            this.ProcessedValue2 = processedValue2;
            this.OriginalValue1 = originalValue1;
            this.OriginalValue2 = originalValue2;
            this.InitialResult = result;
        }

        public override Type CompareType
        {
            get
            {
                return typeof(T);
            }
        }
    }

    [Flags]
    public enum CompareWithComparisonResult
    {
        ValuesNotEqualDefault = 0x1,
        ValuesEqualDefault = 0x2,
        OneValueNull = 0x5,
        ParsedValueComparison = 0x8,
        ParsedValueEquals = 0x12,
        ValuesNonNullEqual = 0x22,
        BothValuesNull = 0x46,
        Value1NullValue2NotNull = 0x85,
        Value1NotNullValue2Null = 0x105,
        ParsedValue1EqualsValue2 = 0x23A,
        ParsedValue2EqualsValue1 = 0x43A
    }

    public enum CompareWithContinuation
    {
        Continue,
        ReturnFalse,
        ReturnTrue
    }
}
