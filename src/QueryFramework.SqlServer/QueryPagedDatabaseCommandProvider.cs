namespace QueryFramework.SqlServer;

public class QueryPagedDatabaseCommandProvider : IPagedDatabaseCommandProvider<ISingleEntityQuery>
{
    private readonly IQueryFieldProvider _fieldProvider;
    private readonly IPagedDatabaseEntityRetrieverSettings _settings;
    private readonly IQueryExpressionEvaluator _evaluator;

    public QueryPagedDatabaseCommandProvider(IQueryFieldProvider fieldProvider,
                                             IPagedDatabaseEntityRetrieverSettings settings,
                                             IQueryExpressionEvaluator evaluator)
    {
        _fieldProvider = fieldProvider;
        _settings = settings;
        _evaluator = evaluator;
    }

    public IPagedDatabaseCommand CreatePaged(ISingleEntityQuery source, DatabaseOperation operation, int offset, int pageSize)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        var fieldSelectionQuery = source as IFieldSelectionQuery;
        var groupingQuery = source as IGroupingQuery;
        var parameterizedQuery = source as IParameterizedQuery;
        return new PagedSelectCommandBuilder()
            .Select(_settings, _fieldProvider, fieldSelectionQuery, _evaluator)
            .Top(source, _settings)
            .Offset(source)
            .Distinct(fieldSelectionQuery)
            .From(source, _settings)
            .Where(source, _settings, _fieldProvider, _evaluator, out int paramCounter)
            .GroupBy(groupingQuery, _settings, _fieldProvider, _evaluator)
            .Having(groupingQuery, _settings, _fieldProvider, _evaluator, ref paramCounter)
            .OrderBy(source, _settings, _fieldProvider, _evaluator)
            .WithParameters(parameterizedQuery)
            .Build();
    }
}
