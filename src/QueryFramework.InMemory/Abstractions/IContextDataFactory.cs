namespace QueryFramework.InMemory.Abstractions;

public interface IContextDataFactory : IDataFactory
{
    IEnumerable<TResult> GetData<TResult>(ISingleEntityQuery query, object? context) where TResult : class;
}
