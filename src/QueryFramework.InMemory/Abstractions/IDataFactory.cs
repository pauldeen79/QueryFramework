namespace QueryFramework.InMemory.Abstractions;

public interface IDataFactory
{
    IEnumerable<TResult> GetData<TResult>(IQuery query) where TResult : class;
}
