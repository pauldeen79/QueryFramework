namespace ExpressionFramework.Core.ExpressionEvaluators;

public class EmptyExpressionEvaluator : IExpressionEvaluator
{
    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluatorCallback callback, out object? result)
    {
        if (expression is IEmptyExpression constantExpression)
        {
            result = default;
            return true;
        }

        result = default;
        return false;
    }
}
