namespace ExpressionFramework.Core.FunctionEvaluators.ConditionEvaluation;

internal class OperatorData
{
    public object? LeftValue { get; }
    public object? RightValue { get; }
    public string LeftValueString => LeftValue == null ? string.Empty : LeftValue.ToString() ?? string.Empty;
    public string RightValueString => RightValue == null ? string.Empty : RightValue.ToString() ?? string.Empty;

    public OperatorData(object? leftValue, object? rightValue)
    {
        LeftValue = leftValue;
        RightValue = rightValue;
    }
}
