namespace QueryFramework.SqlServer;

public class QueryDatabaseCommandProvider<TQuery> : IDatabaseCommandProvider<TQuery>
    where TQuery : ISingleEntityQuery
{
    private readonly IPagedDatabaseCommandProvider<TQuery> _pagedDatabaseCommandProvider;

    public QueryDatabaseCommandProvider(IPagedDatabaseCommandProvider<TQuery> pagedDatabaseCommandProvider)
        => _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;

    public IDatabaseCommand Create(TQuery source, DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        return _pagedDatabaseCommandProvider.CreatePaged(source, operation, 0, 0).DataCommand;
    }
}
