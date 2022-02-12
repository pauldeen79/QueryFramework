namespace ExpressionFramework.Core.FunctionParsers;

public class RightFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, string fieldName, out object? result)
    {
        if (!(function is RightFunction f))
        {
            result = null;
            return false;
        }

        var stringValue = value == null
            ? string.Empty
            : value.ToString();
        result = f.Length <= stringValue.Length
            ? stringValue.Substring(stringValue.Length - f.Length)
            : stringValue;
        return true;
    }
}
