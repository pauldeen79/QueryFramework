namespace QueryFramework.InMemory.FunctionParsers;

public class LeftFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, object? value, string fieldName, out object? functionResult)
    {
        if (!(function is LeftFunction f))
        {
            functionResult = null;
            return false;
        }

        var stringValue = value == null
            ? string.Empty
            : value.ToString();
        functionResult = stringValue.Substring(0, f.Length <= stringValue.Length ? f.Length : stringValue.Length);
        return true;
    }
}
