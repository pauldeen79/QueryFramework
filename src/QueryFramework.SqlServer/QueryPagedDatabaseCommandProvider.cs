namespace QueryFramework.SqlServer;

public class QueryPagedDatabaseCommandProvider : IPagedDatabaseCommandProvider<ISingleEntityQuery>
{
    private readonly IQueryFieldProvider _fieldProvider;
    private readonly IPagedDatabaseEntityRetrieverSettingsFactory _settingsFactory;
    private readonly IQueryExpressionEvaluator _evaluator;

    public QueryPagedDatabaseCommandProvider(IQueryFieldProvider fieldProvider,
                                             IPagedDatabaseEntityRetrieverSettingsFactory settingsFactory,
                                             IQueryExpressionEvaluator evaluator)
    {
        _fieldProvider = fieldProvider;
        _settingsFactory = settingsFactory;
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
        var settings = _settingsFactory.Create(source);
        return new PagedSelectCommandBuilder()
            .Select(settings, _fieldProvider, fieldSelectionQuery, _evaluator)
            .Top(source, settings)
            .Offset(source)
            .Distinct(fieldSelectionQuery)
            .From(source, settings)
            .Where(source, settings, _fieldProvider, _evaluator, out int paramCounter)
            .GroupBy(groupingQuery, settings, _fieldProvider, _evaluator)
            .Having(groupingQuery, settings, _fieldProvider, _evaluator, ref paramCounter)
            .OrderBy(source, settings, _fieldProvider, _evaluator)
            .WithParameters(parameterizedQuery)
            .Build();
    }
}
