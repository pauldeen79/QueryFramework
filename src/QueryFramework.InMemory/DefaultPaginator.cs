namespace QueryFramework.InMemory;

public class DefaultPaginator : IPaginator
{
    public IEnumerable<T> GetPagedData<T>(ISingleEntityQuery query, IEnumerable<T> filteredRecords)
        where T : class
    {
        IEnumerable<T> result = filteredRecords;

        if (query.OrderByFields.Any())
        {
            result = result.OrderBy(x => new OrderByWrapper(x, query.OrderByFields));
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
