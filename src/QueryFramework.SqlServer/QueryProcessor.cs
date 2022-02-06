namespace QueryFramework.SqlServer;

public class QueryProcessor<TResult> : IQueryProcessor
    where TResult : class
{
    private readonly IDatabaseEntityRetriever<TResult> _retriever;
    private readonly IPagedDatabaseCommandProvider<ISingleEntityQuery> _databaseCommandProvider;

    public QueryProcessor(IDatabaseEntityRetriever<TResult> retriever,
                          IPagedDatabaseCommandProvider<ISingleEntityQuery> databaseCommandProvider)
    {
        _retriever = retriever;
        _databaseCommandProvider = databaseCommandProvider;
    }

    public IReadOnlyCollection<T> FindMany<T>(ISingleEntityQuery query)
        where T : class
        => (IReadOnlyCollection<T>)_retriever.FindMany(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand);

    public T? FindOne<T>(ISingleEntityQuery query)
        where T : class
        => _retriever.FindOne(GenerateCommand(query, 1).DataCommand) as T;

    public IPagedResult<T> FindPaged<T>(ISingleEntityQuery query)
        where T : class
        => (IPagedResult<T>)_retriever.FindPaged(GenerateCommand(query, query.Limit.GetValueOrDefault()));

    private IPagedDatabaseCommand GenerateCommand(ISingleEntityQuery query, int limit)
        => _databaseCommandProvider.CreatePaged
        (
            query.Validate(),
            DatabaseOperation.Select,
            query.Offset.GetValueOrDefault(),
            limit
        );
}
