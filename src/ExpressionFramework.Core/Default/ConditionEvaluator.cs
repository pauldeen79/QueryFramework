namespace ExpressionFramework.Core.Default;

public class ConditionEvaluator : IConditionEvaluator
{
    private readonly IExpressionEvaluatorCallback _expressionEvaluatorCallback;

    public ConditionEvaluator(IExpressionEvaluatorCallback expressionEvaluatorCallback)
        => _expressionEvaluatorCallback = expressionEvaluatorCallback;

    public bool IsItemValid(object? item, ICondition condition)
    {
        var leftValue = _expressionEvaluatorCallback.Evaluate(item, condition.LeftExpression);
        var rightValue = _expressionEvaluatorCallback.Evaluate(item, condition.RightExpression);
        
        if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
        {
            return predicate.Invoke(new OperatorData(leftValue, rightValue));
        }

        throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported operator: {condition.Operator}");
    }


}
