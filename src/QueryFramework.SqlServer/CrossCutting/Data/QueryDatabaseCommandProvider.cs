﻿namespace QueryFramework.SqlServer.CrossCutting.Data;

public class QueryDatabaseCommandProvider : IDatabaseCommandProvider<IQuery>
{
    private readonly IPagedDatabaseCommandProvider<IQuery> _pagedDatabaseCommandProvider;

    public QueryDatabaseCommandProvider(IPagedDatabaseCommandProvider<IQuery> pagedDatabaseCommandProvider)
        => _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;

    public IDatabaseCommand Create(IQuery source, DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        return _pagedDatabaseCommandProvider.CreatePaged(source, operation, 0, 0).DataCommand;
    }
}
