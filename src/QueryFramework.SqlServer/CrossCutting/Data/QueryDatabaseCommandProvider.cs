namespace QueryFramework.SqlServer.CrossCutting.Data;

public class QueryDatabaseCommandProvider : IContextDatabaseCommandProvider<ISingleEntityQuery>
{
    private readonly IContextPagedDatabaseCommandProvider<ISingleEntityQuery> _pagedDatabaseCommandProvider;

    public QueryDatabaseCommandProvider(IContextPagedDatabaseCommandProvider<ISingleEntityQuery> pagedDatabaseCommandProvider)
        => _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;

    public IDatabaseCommand Create(ISingleEntityQuery source, DatabaseOperation operation)
        => Create(source, operation, default);

    public IDatabaseCommand Create(ISingleEntityQuery source, DatabaseOperation operation, object? context)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        return _pagedDatabaseCommandProvider.CreatePaged(source, operation, 0, 0, context).DataCommand;
    }
}
