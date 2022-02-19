namespace ExpressionFramework.Core.ExpressionEvaluatorProviders;

public class DelegateExpressionEvaluatorProvider : IExpressionEvaluatorProvider
{
    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluator evaluator, out object? result)
    {
        if (expression is IDelegateExpression delegateExpression)
        {
            result = delegateExpression.ValueDelegate.Invoke(item, expression, evaluator);
            return true;
        }

        result = default;
        return false;
    }
}
