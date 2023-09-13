namespace QueryFramework.InMemory.Abstractions;

public interface IDataProvider
{
    bool TryGetData<TResult>(IQuery query, out IEnumerable<TResult>? result) where TResult : class;
}
