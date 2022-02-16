namespace ExpressionFramework.Core.ExpressionEvaluatorProviders;

public class ConstantExpressionEvaluatorProvider : IExpressionEvaluatorProvider
{
    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluator evaluator, out object? result)
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
