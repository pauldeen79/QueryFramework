namespace QueryFramework.Abstractions;

public interface IContextDataProvider : IDataProvider
{
    bool TryGetData<TResult>(IQuery query, object? context, out IEnumerable<TResult>? result) where TResult : class;
}
