namespace ExpressionFramework.Core.FunctionEvaluators;

public class CountFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        result = null;
        if (!(function is CountFunction))
        {
            return false;
        }

        var enumerable = value as IEnumerable;
        if (enumerable != null)
        {
            result = enumerable.OfType<object>().Count();
        }

        return true;
    }
}
