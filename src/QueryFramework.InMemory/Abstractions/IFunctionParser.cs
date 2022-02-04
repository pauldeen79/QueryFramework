namespace QueryFramework.InMemory.Abstractions;

public interface IFunctionParser
{
    bool TryParse(IQueryExpressionFunction function, object? value, string fieldName, out object? functionResult);
}
