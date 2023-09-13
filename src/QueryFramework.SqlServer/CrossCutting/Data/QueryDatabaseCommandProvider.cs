namespace QueryFramework.SqlServer.CrossCutting.Data;

public class QueryDatabaseCommandProvider : IContextDatabaseCommandProvider<IQuery>
{
    private readonly IContextPagedDatabaseCommandProvider<IQuery> _pagedDatabaseCommandProvider;

    public QueryDatabaseCommandProvider(IContextPagedDatabaseCommandProvider<IQuery> pagedDatabaseCommandProvider)
        => _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;

    public IDatabaseCommand Create(IQuery source, DatabaseOperation operation)
        => Create(source, operation, default);

    public IDatabaseCommand Create(IQuery source, DatabaseOperation operation, object? context)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        return _pagedDatabaseCommandProvider.CreatePaged(source, operation, 0, 0, context).DataCommand;
    }
}
