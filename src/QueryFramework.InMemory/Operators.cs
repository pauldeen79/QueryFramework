using System;
using System.Collections.Generic;
using QueryFramework.Abstractions;

namespace QueryFramework.InMemory
{
    internal static class Operators
    {
        internal static readonly Dictionary<QueryOperator, Predicate<OperatorData>> Items = new Dictionary<QueryOperator, Predicate<OperatorData>>
        {
            { QueryOperator.Contains, Contains },
            { QueryOperator.NotContains, data => !Contains(data) },
            { QueryOperator.StartsWith, StartsWith},
            { QueryOperator.NotStartsWith, data => !StartsWith(data) },
            { QueryOperator.EndsWith, EndsWith},
            { QueryOperator.NotEndsWith, data => !EndsWith(data) },
            { QueryOperator.Equal, Equal },
            { QueryOperator.NotEqual, data => !Equal(data) },
            { QueryOperator.GreaterOrEqual, GreaterOrEqual },
            { QueryOperator.Greater, Greater },
            { QueryOperator.LowerOrEqual, LowerOrEqual },
            { QueryOperator.Lower, Lower },
            { QueryOperator.IsNull, Null },
            { QueryOperator.IsNotNull, data => !Null(data) },
            { QueryOperator.IsNullOrEmpty, NullOrEmpty },
            { QueryOperator.IsNotNullOrEmpty, data => !NullOrEmpty(data) },
        };

        private static bool Null(OperatorData data)
            => data.Value == null;

        private static bool NullOrEmpty(OperatorData data)
            => data.Value == null
            || data.Value.ToString() == string.Empty;

        private static bool Lower(OperatorData data)
            => data.Value != null
            && data.ConditionValue != null
            && data.Value is IComparable c
            && c.CompareTo(data.ConditionValue) < 0;

        private static bool LowerOrEqual(OperatorData data)
            => data.Value != null
            && data.ConditionValue != null
            && data.Value is IComparable c
            && c.CompareTo(data.ConditionValue) <= 0;

        private static bool Greater(OperatorData data)
            => data.Value != null
            && data.ConditionValue != null
            && data.Value is IComparable c
            && c.CompareTo(data.ConditionValue) > 0;

        private static bool GreaterOrEqual(OperatorData data)
            => data.Value != null
            && data.ConditionValue != null
            && data.Value is IComparable c
            && c.CompareTo(data.ConditionValue) >= 0;

        private static bool Equal(OperatorData data)
            => (data.Value == null && data.ConditionValue == null)
            || (data.Value is string s && data.ConditionValueString != null && s.Equals(data.ConditionValueString, StringComparison.OrdinalIgnoreCase))
            || (data.Value != null && data.ConditionValue != null && data.Value.Equals(data.ConditionValue));

        private static bool StartsWith(OperatorData data)
            => data.Value != null
            && !string.IsNullOrEmpty(data.ConditionValueString)
            && data.Value is string s
            && s.StartsWith(data.ConditionValueString, StringComparison.CurrentCultureIgnoreCase);

        private static bool EndsWith(OperatorData data)
            => data.Value != null
            && !string.IsNullOrEmpty(data.ConditionValueString)
            && data.Value is string s
            && s.EndsWith(data.ConditionValueString, StringComparison.CurrentCultureIgnoreCase);

        private static bool Contains(OperatorData data)
            => data.Value != null
            && !string.IsNullOrEmpty(data.ConditionValueString)
            && data.Value is string s
            && s.IndexOf(data.ConditionValueString, StringComparison.CurrentCultureIgnoreCase) > -1;
    }
}
