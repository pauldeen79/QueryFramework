namespace QueryFramework.SqlServer;

public class QueryProcessor : IQueryProcessor
{
    private readonly IEnumerable<IDatabaseEntityRetrieverProvider> _databaseEntityRetrieverProviders;
    private readonly IPagedDatabaseCommandProvider<ISingleEntityQuery> _databaseCommandProvider;

    public QueryProcessor(IEnumerable<IDatabaseEntityRetrieverProvider> databaseEntityRetrieverProviders,
                          IPagedDatabaseCommandProvider<ISingleEntityQuery> databaseCommandProvider)
    {
        _databaseEntityRetrieverProviders = databaseEntityRetrieverProviders;
        _databaseCommandProvider = databaseCommandProvider;
    }

    public IReadOnlyCollection<T> FindMany<T>(ISingleEntityQuery query)
        where T : class
        => GetRetriever<T>().FindMany(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand);

    public T? FindOne<T>(ISingleEntityQuery query)
        where T : class
        => GetRetriever<T>().FindOne(GenerateCommand(query, 1).DataCommand);

    public IPagedResult<T> FindPaged<T>(ISingleEntityQuery query)
        where T : class
        => GetRetriever<T>().FindPaged(GenerateCommand(query, query.Limit.GetValueOrDefault()));

    private IPagedDatabaseCommand GenerateCommand(ISingleEntityQuery query, int limit)
        => _databaseCommandProvider.CreatePaged
        (
            query.Validate(),
            DatabaseOperation.Select,
            query.Offset.GetValueOrDefault(),
            limit
        );

    private IDatabaseEntityRetriever<TResult> GetRetriever<TResult>()
        where TResult : class
        => _databaseEntityRetrieverProviders
            .Select(x => x.GetRetriever<TResult>())
            .FirstOrDefault(x => x != null)
                ?? throw new InvalidOperationException($"Data type {typeof(TResult).FullName} does not have a database entity retriever provider");
}
