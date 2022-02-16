namespace ExpressionFramework.Core.FunctionEvaluators;

public class LengthFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        if (!(function is LengthFunction))
        {
            result = null;
            return false;
        }

        result = value == null
            ? 0
            : value.ToString().Length;
        return true;
    }
}
