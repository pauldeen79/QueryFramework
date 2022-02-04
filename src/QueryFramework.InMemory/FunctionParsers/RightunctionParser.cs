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
        functionResult = stringValue.Substring(stringValue.Length - f.Length);
        return true;
    }
}
