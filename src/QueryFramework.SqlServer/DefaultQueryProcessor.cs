namespace QueryFramework.SqlServer;

public class DefaultQueryProcessor : IQueryProcessor
{
    private readonly IDatabaseEntityRetrieverFactory _databaseEntityRetrieverFactory;
    private readonly IPagedDatabaseCommandProvider<IQuery> _databaseCommandProvider;

    public DefaultQueryProcessor(IDatabaseEntityRetrieverFactory databaseEntityRetrieverFactory,
                                 IPagedDatabaseCommandProvider<IQuery> databaseCommandProvider)
    {
        _databaseEntityRetrieverFactory = databaseEntityRetrieverFactory;
        _databaseCommandProvider = databaseCommandProvider;
    }

    public IReadOnlyCollection<T> FindMany<T>(IQuery query)
        where T : class
        => GetRetriever<T>(query).FindMany(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand);

    public T? FindOne<T>(IQuery query)
        where T : class
        => GetRetriever<T>(query).FindOne(GenerateCommand(query, 1).DataCommand);

    public IPagedResult<T> FindPaged<T>(IQuery query)
        where T : class
        => GetRetriever<T>(query).FindPaged(GenerateCommand(query, query.Limit.GetValueOrDefault()));

    private IPagedDatabaseCommand GenerateCommand(IQuery query, int limit)
        => _databaseCommandProvider.CreatePaged
        (
            query.Validate(),
            DatabaseOperation.Select,
            query.Offset.GetValueOrDefault(),
            limit
        );

    private IDatabaseEntityRetriever<TResult> GetRetriever<TResult>(IQuery query)
        where TResult : class
        => _databaseEntityRetrieverFactory.Create<TResult>(query);
}
