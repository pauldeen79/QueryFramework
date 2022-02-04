namespace QueryFramework.InMemory.Abstractions;

public interface IExpressionEvaluator
{
    object? GetValue(object item, IQueryExpression field);
}
