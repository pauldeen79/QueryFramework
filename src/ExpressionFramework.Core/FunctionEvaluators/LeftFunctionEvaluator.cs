namespace ExpressionFramework.Core.FunctionEvaluators;

public class LeftFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        if (!(function is LeftFunction f))
        {
            result = null;
            return false;
        }

        var stringValue = value == null
            ? string.Empty
            : value.ToString();
        result = stringValue.Substring(0, f.Length <= stringValue.Length ? f.Length : stringValue.Length);
        return true;
    }
}
