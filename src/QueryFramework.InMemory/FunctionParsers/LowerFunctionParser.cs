namespace QueryFramework.InMemory.FunctionParsers;

public class LowerFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, object? value, string fieldName, out object? functionResult)
    {
        if (!(function is LowerFunction))
        {
            functionResult = null;
            return false;
        }

        functionResult = value == null
            ? 0
            : value.ToString().ToLowerInvariant();
        return true;
    }
}
