namespace ExpressionFramework.Core.FunctionEvaluators;

public class UpperFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        if (!(function is UpperFunction))
        {
            result = null;
            return false;
        }

        result = value == null
            ? string.Empty
            : value.ToString().ToUpperInvariant();
        return true;
    }
}
