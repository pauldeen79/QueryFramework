namespace QueryFramework.InMemory.Abstractions;

public interface IPaginator
{
    IEnumerable<T> GetPagedData<T>(ISingleEntityQuery query, IEnumerable<T> filteredRecords)
        where T : class;
}
