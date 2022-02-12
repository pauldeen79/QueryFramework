namespace ExpressionFramework.Core.FunctionParsers;

public class TrimFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, string fieldName, out object? result)
    {
        if (!(function is TrimFunction))
        {
            result = null;
            return false;
        }

        result = value == null
            ? string.Empty
            : value.ToString().Trim();
        return true;
    }
}
