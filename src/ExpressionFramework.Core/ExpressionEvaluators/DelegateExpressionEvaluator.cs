namespace ExpressionFramework.Core.ExpressionEvaluators;

public class DelegateExpressionEvaluator : IExpressionEvaluator
{
    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluatorCallback callback, out object? result)
    {
        if (expression is IDelegateExpression delegateExpression)
        {
            result = delegateExpression.ValueDelegate.Invoke();
            return true;
        }

        result = default;
        return false;
    }
}
