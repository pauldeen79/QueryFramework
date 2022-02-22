namespace QueryFramework.InMemory;

public class DefaultPaginator : IPaginator
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    public DefaultPaginator(IExpressionEvaluator expressionEvaluator) => _expressionEvaluator = expressionEvaluator;

    public IEnumerable<T> GetPagedData<T>(ISingleEntityQuery query, IEnumerable<T> filteredRecords)
        where T : class
    {
        IEnumerable<T> result = filteredRecords;

        if (query.OrderByFields.Any())
        {
            result = result.OrderBy(x => new OrderByWrapper(x, query.OrderByFields, _expressionEvaluator));
        }

        if (query.Offset != null)
        {
            result = result.Skip(query.Offset.Value);
        }
        if (query.Limit != null)
        {
            result = result.Take(query.Limit.Value);
        }

        return result;
    }
}
