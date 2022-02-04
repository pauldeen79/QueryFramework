namespace QueryFramework.InMemory.FunctionParsers;

public class TrimFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, object? value, string fieldName, out object? functionResult)
    {
        if (!(function is TrimFunction))
        {
            functionResult = null;
            return false;
        }

        functionResult = value == null
            ? 0
            : value.ToString().Trim();
        return true;
    }
}
