namespace QueryFramework.SqlServer;

public class QueryDatabaseCommandProvider : IDatabaseCommandProvider<ISingleEntityQuery>
{
    private readonly IPagedDatabaseCommandProvider<ISingleEntityQuery> _pagedDatabaseCommandProvider;

    public QueryDatabaseCommandProvider(IPagedDatabaseCommandProvider<ISingleEntityQuery> pagedDatabaseCommandProvider)
        => _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;

    public IDatabaseCommand Create(ISingleEntityQuery source, DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        return _pagedDatabaseCommandProvider.CreatePaged(source, operation, 0, 0).DataCommand;
    }
}
