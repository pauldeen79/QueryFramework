namespace QueryFramework.InMemory.FunctionParsers;

public class UpperFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, object? value, string fieldName, out object? functionResult)
    {
        if (!(function is UpperFunction))
        {
            functionResult = null;
            return false;
        }

        functionResult = value == null
            ? 0
            : value.ToString().ToUpperInvariant();
        return true;
    }
}
