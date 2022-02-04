namespace QueryFramework.SqlServer;

public class QueryPagedDatabaseCommandProvider<TQuery> : IPagedDatabaseCommandProvider<TQuery>
    where TQuery : ISingleEntityQuery
{
    private IQueryFieldProvider FieldProvider { get; }
    private IPagedDatabaseEntityRetrieverSettings Settings { get; }
    private IQueryExpressionEvaluator Evaluator { get; }

    public QueryPagedDatabaseCommandProvider(IQueryFieldProvider fieldProvider,
                                             IPagedDatabaseEntityRetrieverSettings settings,
                                             IQueryExpressionEvaluator evaluator)
    {
        FieldProvider = fieldProvider;
        Settings = settings;
        Evaluator = evaluator;
    }

    public IPagedDatabaseCommand CreatePaged(TQuery source, DatabaseOperation operation, int offset, int pageSize)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        var fieldSelectionQuery = source as IFieldSelectionQuery;
        var groupingQuery = source as IGroupingQuery;
        var parameterizedQuery = source as IParameterizedQuery;
        return new PagedSelectCommandBuilder()
            .Select(Settings, FieldProvider, fieldSelectionQuery, Evaluator)
            .Top(source, Settings)
            .Offset(source)
            .Distinct(fieldSelectionQuery)
            .From(source, Settings)
            .Where(source, Settings, FieldProvider, Evaluator, out int paramCounter)
            .GroupBy(groupingQuery, Settings, FieldProvider, Evaluator)
            .Having(groupingQuery, Settings, FieldProvider, Evaluator, ref paramCounter)
            .OrderBy(source, Settings, FieldProvider, Evaluator)
            .WithParameters(parameterizedQuery)
            .Build();
    }
}
