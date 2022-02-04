namespace QueryFramework.InMemory.FunctionParsers;

public class LengthFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, object? value, string fieldName, out object? functionResult)
    {
        if (!(function is LengthFunction))
        {
            functionResult = null;
            return false;
        }

        functionResult = value == null
            ? 0
            : value.ToString().Length;
        return true;
    }
}
