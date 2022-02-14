namespace ExpressionFramework.Core.ExpressionEvaluators;

public class ConstantExpressionEvaluator : IExpressionEvaluator
{
    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluatorCallback callback, out object? result)
    {
        if (expression is IConstantExpression constantExpression)
        {
            result = constantExpression.Value;
            return true;
        }

        result = default;
        return false;
    }
}
