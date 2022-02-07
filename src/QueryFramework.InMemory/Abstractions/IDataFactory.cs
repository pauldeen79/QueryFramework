namespace QueryFramework.InMemory.Abstractions;

public interface IDataFactory
{
    IEnumerable<TResult> GetData<TResult>(ISingleEntityQuery query) where TResult : class;
}
