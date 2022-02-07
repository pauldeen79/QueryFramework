namespace QueryFramework.SqlServer;

public class DefaultQueryProcessor : IQueryProcessor
{
    private readonly IDatabaseEntityRetrieverFactory _databaseEntityRetrieverFactory;
    private readonly IPagedDatabaseCommandProviderFactory _databaseCommandProviderFactory;

    public DefaultQueryProcessor(IDatabaseEntityRetrieverFactory databaseEntityRetrieverFactory,
                                 IPagedDatabaseCommandProviderFactory databaseCommandProviderFactory)
    {
        _databaseEntityRetrieverFactory = databaseEntityRetrieverFactory;
        _databaseCommandProviderFactory = databaseCommandProviderFactory;
    }

    public IReadOnlyCollection<T> FindMany<T>(ISingleEntityQuery query)
        where T : class
        => GetRetriever<T>(query).FindMany(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand);

    public T? FindOne<T>(ISingleEntityQuery query)
        where T : class
        => GetRetriever<T>(query).FindOne(GenerateCommand(query, 1).DataCommand);

    public IPagedResult<T> FindPaged<T>(ISingleEntityQuery query)
        where T : class
        => GetRetriever<T>(query).FindPaged(GenerateCommand(query, query.Limit.GetValueOrDefault()));

    private IPagedDatabaseCommand GenerateCommand(ISingleEntityQuery query, int limit)
        => _databaseCommandProviderFactory.Create(query.Validate()).CreatePaged
        (
            query,
            DatabaseOperation.Select,
            query.Offset.GetValueOrDefault(),
            limit
        );

    private IDatabaseEntityRetriever<TResult> GetRetriever<TResult>(ISingleEntityQuery query)
        where TResult : class
        => _databaseEntityRetrieverFactory.Create<TResult>(query);
}
