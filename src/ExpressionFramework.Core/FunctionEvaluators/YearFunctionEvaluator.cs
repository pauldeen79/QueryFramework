namespace ExpressionFramework.Core.FunctionEvaluators;

public class YearFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluatorCallback callback, out object? result)
    {
        result = null;
        if (!(function is YearFunction))
        {
            return false;
        }

        if (value is DateTime dateTime)
        {
            result = dateTime.Year;
        }

        return true;
    }
}
