namespace QueryFramework.InMemory.Abstractions;

public interface IPaginator
{
    IEnumerable<T> GetPagedData<T>(IQuery query, IEnumerable<T> filteredRecords)
        where T : class;
}
