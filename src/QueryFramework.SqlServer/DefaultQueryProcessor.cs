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

    public IReadOnlyCollection<TResult> FindMany<TResult>(IQuery query)
        where TResult : class
        => GetRetriever<TResult>(query).FindMany(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand);

    public Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class
        => GetRetriever<TResult>(query).FindManyAsync(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand, cancellationToken);

    public TResult? FindOne<TResult>(IQuery query)
        where TResult : class
        => GetRetriever<TResult>(query).FindOne(GenerateCommand(query, 1).DataCommand);

    public Task<TResult?> FindOneAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class
        => GetRetriever<TResult>(query).FindOneAsync(GenerateCommand(query, 1).DataCommand, cancellationToken);

    public IPagedResult<TResult> FindPaged<TResult>(IQuery query)
        where TResult : class
        => GetRetriever<TResult>(query).FindPaged(GenerateCommand(query, query.Limit.GetValueOrDefault()));

    public Task<IPagedResult<TResult>> FindPagedAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class
        => GetRetriever<TResult>(query).FindPagedAsync(GenerateCommand(query, query.Limit.GetValueOrDefault()), cancellationToken);

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
