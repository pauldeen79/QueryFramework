namespace ExpressionFramework.Core.FunctionEvaluators.ConditionEvaluation;

internal static class Operators
{
    internal static readonly Dictionary<Operator, Predicate<OperatorData>> Items = new Dictionary<Operator, Predicate<OperatorData>>
    {
        { Operator.Contains, Contains },
        { Operator.NotContains, data => data.LeftValue != null && !Contains(data) },
        { Operator.StartsWith, StartsWith},
        { Operator.NotStartsWith, data => data.LeftValue != null && !StartsWith(data) },
        { Operator.EndsWith, EndsWith},
        { Operator.NotEndsWith, data => data.LeftValue != null && !EndsWith(data) },
        { Operator.Equal, Equal },
        { Operator.NotEqual, data => !Equal(data) },
        { Operator.GreaterOrEqual, GreaterOrEqual },
        { Operator.Greater, Greater },
        { Operator.SmallerOrEqual, SmallerOrEqual },
        { Operator.Smaller, Smaller },
        { Operator.IsNull, Null },
        { Operator.IsNotNull, data => !Null(data) },
        { Operator.IsNullOrEmpty, NullOrEmpty },
        { Operator.IsNotNullOrEmpty, data => !NullOrEmpty(data) },
        { Operator.IsNullOrWhiteSpace, NullOrWhiteSpace },
        { Operator.IsNotNullOrWhiteSpace, data => !NullOrWhiteSpace(data) },
    };

    private static bool Null(OperatorData data)
        => data.LeftValue == null;

    private static bool NullOrEmpty(OperatorData data)
        => data.LeftValue == null
        || data.LeftValue.ToString() == string.Empty;

    private static bool NullOrWhiteSpace(OperatorData data)
        => data.LeftValue == null
        || data.LeftValue.ToString().Trim() == string.Empty;

    private static bool Smaller(OperatorData data)
        => data.LeftValue != null
        && data.RightValue != null
        && data.LeftValue is IComparable c
        && c.CompareTo(data.RightValue) < 0;

    private static bool SmallerOrEqual(OperatorData data)
        => data.LeftValue != null
        && data.RightValue != null
        && data.LeftValue is IComparable c
        && c.CompareTo(data.RightValue) <= 0;

    private static bool Greater(OperatorData data)
        => data.LeftValue != null
        && data.RightValue != null
        && data.LeftValue is IComparable c
        && c.CompareTo(data.RightValue) > 0;

    private static bool GreaterOrEqual(OperatorData data)
        => data.LeftValue != null
        && data.RightValue != null
        && data.LeftValue is IComparable c
        && c.CompareTo(data.RightValue) >= 0;

    private static bool Equal(OperatorData data)
        => data.LeftValue == null && data.RightValue == null
        || data.LeftValueString.Equals(data.RightValueString, StringComparison.OrdinalIgnoreCase)
        || data.LeftValue != null && data.RightValue != null && data.LeftValue.Equals(data.RightValue);

    private static bool StartsWith(OperatorData data)
        => data.LeftValue != null
        && !string.IsNullOrEmpty(data.RightValueString)
        && data.LeftValueString.StartsWith(data.RightValueString, StringComparison.CurrentCultureIgnoreCase);

    private static bool EndsWith(OperatorData data)
        => data.LeftValue != null
        && !string.IsNullOrEmpty(data.RightValueString)
        && data.LeftValueString.EndsWith(data.RightValueString, StringComparison.CurrentCultureIgnoreCase);

    private static bool Contains(OperatorData data)
        => data.LeftValue != null
        && !string.IsNullOrEmpty(data.RightValueString)
        && data.LeftValueString.IndexOf(data.RightValueString, StringComparison.CurrentCultureIgnoreCase) > -1;
}
