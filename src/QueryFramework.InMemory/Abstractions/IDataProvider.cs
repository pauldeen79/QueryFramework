namespace QueryFramework.InMemory.Abstractions;

public interface IDataProvider
{
    bool TryGetData<TResult>(ISingleEntityQuery query, out IEnumerable<TResult>? result) where TResult : class;
}
