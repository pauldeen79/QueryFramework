namespace ExpressionFramework.Core.ExpressionEvaluatorProviders;

public class EmptyExpressionEvaluatorProvider : IExpressionEvaluatorProvider
{
    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluator evaluator, out object? result)
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
