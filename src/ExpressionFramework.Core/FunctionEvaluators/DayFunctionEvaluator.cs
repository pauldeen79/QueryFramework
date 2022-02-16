namespace ExpressionFramework.Core.FunctionEvaluators;

public class DayFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        result = null;
        if (!(function is DayFunction))
        {
            return false;
        }

        if (value is DateTime dateTime)
        {
            result = dateTime.Day;
        }

        return true;
    }
}
