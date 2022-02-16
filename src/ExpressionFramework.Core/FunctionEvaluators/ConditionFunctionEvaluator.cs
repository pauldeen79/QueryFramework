namespace ExpressionFramework.Core.FunctionEvaluators;

public class ConditionFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluatorCallback callback, out object? result)
    {
        if (!(function is ConditionFunction c))
        {
            result = null;
            return false;
        }

        result = IsItemValid(value, c.Condition, callback);
        return true;
    }

    private bool IsItemValid(object? item, ICondition condition, IExpressionEvaluatorCallback callback)
    {
        var leftValue = callback.Evaluate(item, condition.LeftExpression);
        var rightValue = callback.Evaluate(item, condition.RightExpression);

        if (Operators.Items.TryGetValue(condition.Operator, out var predicate))
        {
            return predicate.Invoke(new OperatorData(leftValue, rightValue));
        }

        throw new ArgumentOutOfRangeException(nameof(condition), $"Unsupported operator: {condition.Operator}");
    }
}
