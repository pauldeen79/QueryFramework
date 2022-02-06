namespace QueryFramework.InMemory;

public class QueryProcessor : IQueryProcessor
{
    private readonly IPaginator _paginator;
    private readonly IEnumerable<IDataProvider> _dataProviders;

    public QueryProcessor(IPaginator paginator, IEnumerable<IDataProvider> dataProviders)
    {
        _paginator = paginator;
        _dataProviders = dataProviders;
    }

    public IReadOnlyCollection<TResult> FindMany<TResult>(ISingleEntityQuery query)
        where TResult : class
        => _paginator.GetPagedData
        (
            new SingleEntityQuery(null, null, query.Conditions, query.OrderByFields),
            GetData<TResult>(query)
        ).ToList();

    public TResult? FindOne<TResult>(ISingleEntityQuery query) where TResult : class
        => _paginator.GetPagedData
        (
            new SingleEntityQuery(null, null, query.Conditions, query.OrderByFields),
            GetData<TResult>(query)
        ).FirstOrDefault();

    public IPagedResult<TResult> FindPaged<TResult>(ISingleEntityQuery query)
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

    private IEnumerable<TResult> GetData<TResult>(ISingleEntityQuery query)
        where TResult : class
        => _dataProviders
            .Select(x => x.GetData<TResult>(query))
            .FirstOrDefault(x => x != null)
            ?? throw new InvalidOperationException($"Data type [{typeof(TResult).FullName}] does not have a data provider");
}
