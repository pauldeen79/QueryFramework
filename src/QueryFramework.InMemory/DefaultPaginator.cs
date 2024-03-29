﻿namespace QueryFramework.InMemory;

public class DefaultPaginator : IPaginator
{
    public IEnumerable<T> GetPagedData<T>(IQuery query, IEnumerable<T> filteredRecords)
        where T : class
    {
        IEnumerable<T> result = filteredRecords;

        if (query.OrderByFields.Any())
        {
            result = result.OrderBy(x => new OrderByWrapper(x, query.OrderByFields));
        }

        if (query.Offset is not null)
        {
            result = result.Skip(query.Offset.Value);
        }
        if (query.Limit is not null)
        {
            result = result.Take(query.Limit.Value);
        }

        return result;
    }
}
