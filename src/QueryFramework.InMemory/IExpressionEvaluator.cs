namespace QueryFramework.InMemory;

public interface IExpressionEvaluator<in T>
    where T : class
{
    object? GetValue(T item, IQueryExpression field);
}
