using QueryFramework.Abstractions;

namespace QueryFramework.InMemory
{
    internal interface IExpressionEvaluator<in T>
    {
        object GetValue(T item, IQueryExpression field);
    }
}
