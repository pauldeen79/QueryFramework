namespace ExpressionFramework.Core.FunctionEvaluators;

public class SumFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        result = null;
        if (!(function is SumFunction))
        {
            return false;
        }

        var enumerable = value as IEnumerable;
        if (enumerable != null)
        {
            var items = enumerable.OfType<object>().ToArray();
            var types = items.GroupBy(x => x.GetType()).ToArray();
            if (types.Length == 1)
            {
                if (types.First().Key == typeof(int))
                {
                    result = items.Cast<int>().Sum();
                }
                else if (types.First().Key == typeof(long))
                {
                    result = items.Cast<long>().Sum();
                }
                else if (types.First().Key == typeof(decimal))
                {
                    result = items.Cast<decimal>().Sum();
                }
                else if (types.First().Key == typeof(double))
                {
                    result = items.Cast<double>().Sum();
                }
                else if (types.First().Key == typeof(float))
                {
                    result = items.Cast<float>().Sum();
                }
            }
        }

        return true;
    }
}
