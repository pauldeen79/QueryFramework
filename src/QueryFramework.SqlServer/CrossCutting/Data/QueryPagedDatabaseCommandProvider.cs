namespace QueryFramework.SqlServer.CrossCutting.Data;

public class QueryPagedDatabaseCommandProvider : IPagedDatabaseCommandProvider<ISingleEntityQuery>
{
    private readonly IQueryFieldInfoFactory _fieldInfoFactory;
    private readonly IPagedDatabaseEntityRetrieverSettingsFactory _settingsFactory;
    private readonly IQueryExpressionEvaluator _evaluator;

    public QueryPagedDatabaseCommandProvider(IQueryFieldInfoFactory fieldInfoFactory,
                                             IPagedDatabaseEntityRetrieverSettingsFactory settingsFactory,
                                             IQueryExpressionEvaluator evaluator)
    {
        _fieldInfoFactory = fieldInfoFactory;
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
        var fieldInfo = _fieldInfoFactory.Create(source);
        return new PagedSelectCommandBuilder()
            .Select(settings, fieldInfo, fieldSelectionQuery, _evaluator)
            .Top(source, settings)
            .Offset(source)
            .Distinct(fieldSelectionQuery)
            .From(source, settings)
            .Where(source, settings, fieldInfo, _evaluator, out var paramCounter)
            .GroupBy(groupingQuery, fieldInfo, _evaluator)
            .Having(groupingQuery, fieldInfo, _evaluator, ref paramCounter)
            .OrderBy(source, settings, fieldInfo, _evaluator)
            .WithParameters(parameterizedQuery)
            .Build();
    }
}
