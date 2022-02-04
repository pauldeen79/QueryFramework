namespace QueryFramework.InMemory.FunctionParsers;

public class RightFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, object? value, string fieldName, out object? functionResult)
    {
        if (!(function is RightFunction f))
        {
            functionResult = null;
            return false;
        }

        var stringValue = value == null
            ? string.Empty
            : value.ToString();
        functionResult = f.Length <= stringValue.Length
            ? stringValue.Substring(stringValue.Length - f.Length)
            : stringValue;
        return true;
    }
}
