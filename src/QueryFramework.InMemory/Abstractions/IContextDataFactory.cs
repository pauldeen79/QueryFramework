namespace QueryFramework.InMemory.Abstractions;

public interface IContextDataFactory : IDataFactory
{
    IEnumerable<TResult> GetData<TResult>(IQuery query, object? context) where TResult : class;
}
