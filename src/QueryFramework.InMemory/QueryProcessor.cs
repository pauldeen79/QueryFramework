namespace QueryFramework.InMemory;

public class QueryProcessor : IQueryProcessor
{
    private readonly IPaginator _paginator;
    private readonly IDataFactory _dataFactory;

    public QueryProcessor(IPaginator paginator, IDataFactory dataFactory)
    {
        _paginator = paginator;
        _dataFactory = dataFactory;
    }

    public IReadOnlyCollection<TResult> FindMany<TResult>(IQuery query)
        where TResult : class
        => _paginator.GetPagedData
        (
            new SingleEntityQuery(null, null, query.Filter, query.OrderByFields),
            GetData<TResult>(query)
        ).ToList();

    public Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class
        => Task.FromResult(FindMany<TResult>(query));

    public TResult? FindOne<TResult>(IQuery query)
        where TResult : class
        => _paginator.GetPagedData
        (
            new SingleEntityQuery(null, null, query.Filter, query.OrderByFields),
            GetData<TResult>(query)
        ).FirstOrDefault();

    public Task<TResult?> FindOneAsync<TResult>(IQuery query, CancellationToken cancellationToken) where TResult : class
        => Task.FromResult(FindOne<TResult>(query));

    public IPagedResult<TResult> FindPaged<TResult>(IQuery query)
        where TResult : class
    {
        var filteredRecords = GetData<TResult>(query).ToArray();
        return new PagedResult<TResult>
        (
            _paginator.GetPagedData(query, filteredRecords),
            filteredRecords.Length,
            query.Offset.GetValueOrDefault(),
            query.Limit.GetValueOrDefault()
        );
    }

    public Task<IPagedResult<TResult>> FindPagedAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class
        => Task.FromResult(FindPaged<TResult>(query));

    private IEnumerable<TResult> GetData<TResult>(IQuery query)
        where TResult : class
        => _dataFactory is IContextDataFactory c
            ? c.GetData<TResult>(query)
            : _dataFactory.GetData<TResult>(query);
}
