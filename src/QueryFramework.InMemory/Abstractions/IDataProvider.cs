namespace QueryFramework.InMemory.Abstractions;

public interface IDataProvider
{
    IEnumerable<TResult>? GetData<TResult>(ISingleEntityQuery query)
        where TResult : class;
}
