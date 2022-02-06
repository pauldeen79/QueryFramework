namespace QueryFramework.InMemory;

public class QueryProcessor : IQueryProcessor
{
    private readonly Func<IEnumerable> _sourceDataDelegate;
    private readonly IConditionEvaluator _conditionEvaluator;
    private readonly IPaginator _paginator;

    public QueryProcessor(Func<IEnumerable> sourceDataDelegate,
                          IConditionEvaluator conditionEvaluator,
                          IPaginator paginator)
    {
        _sourceDataDelegate = sourceDataDelegate;
        _conditionEvaluator = conditionEvaluator;
        _paginator = paginator;
    }

    public TResult FindOne<TResult>(ISingleEntityQuery query)
        where TResult : class
        => _sourceDataDelegate.Invoke()
            .OfType<TResult>()
            .FirstOrDefault(item => _conditionEvaluator.IsItemValid(item, query.Conditions));

    public IReadOnlyCollection<TResult> FindMany<TResult>(ISingleEntityQuery query)
        where TResult : class
        => _sourceDataDelegate.Invoke()
            .OfType<TResult>()
            .Where(item => _conditionEvaluator.IsItemValid(item, query.Conditions))
            .ToList();

    public IPagedResult<TResult> FindPaged<TResult>(ISingleEntityQuery query)
        where TResult : class
    {
        var filteredRecords = new List<TResult>(_sourceDataDelegate.Invoke()
            .OfType<TResult>()
            .Where(item => _conditionEvaluator.IsItemValid(item, query.Conditions)));
        return new PagedResult<TResult>(_paginator.GetPagedData(query, filteredRecords),
                                        filteredRecords.Count,
                                        query.Offset.GetValueOrDefault(),
                                        query.Limit.GetValueOrDefault());
    }
}
