namespace QueryFramework.SqlServer;

public class QueryDatabaseCommandProvider : IDatabaseCommandProvider<ISingleEntityQuery>
{
    private readonly IPagedDatabaseCommandProviderFactory _pagedDatabaseCommandProviderFactory;

    public QueryDatabaseCommandProvider(IPagedDatabaseCommandProviderFactory pagedDatabaseCommandProviderFactory)
        => _pagedDatabaseCommandProviderFactory = pagedDatabaseCommandProviderFactory;

    public IDatabaseCommand Create(ISingleEntityQuery source, DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        return _pagedDatabaseCommandProviderFactory.Create(source).CreatePaged(source, operation, 0, 0).DataCommand;
    }
}
