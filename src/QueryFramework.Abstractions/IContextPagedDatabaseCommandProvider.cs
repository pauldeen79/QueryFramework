namespace QueryFramework.Abstractions;

public interface IContextPagedDatabaseCommandProvider<in TSource> : IPagedDatabaseCommandProvider<TSource>
{
    IPagedDatabaseCommand CreatePaged(TSource source, DatabaseOperation operation, int offset, int pageSize, object? context);
}
