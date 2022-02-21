namespace QueryFramework.SqlServer.CrossCutting.Data;

public class QueryPagedDatabaseCommandProvider : IPagedDatabaseCommandProvider<ISingleEntityQuery>
{
    private readonly IQueryFieldInfoFactory _fieldInfoFactory;
    private readonly IPagedDatabaseEntityRetrieverSettingsFactory _settingsFactory;
    private readonly ISqlExpressionEvaluator _evaluator;

    public QueryPagedDatabaseCommandProvider(IQueryFieldInfoFactory fieldInfoFactory,
                                             IPagedDatabaseEntityRetrieverSettingsFactory settingsFactory,
                                             ISqlExpressionEvaluator evaluator)
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
        var parameterBag = new ParameterBag();
        return new PagedSelectCommandBuilder()
            .Select(settings, fieldInfo, fieldSelectionQuery, _evaluator, parameterBag)
            .Top(source, settings)
            .Offset(source)
            .Distinct(fieldSelectionQuery)
            .From(source, settings)
            .Where(source, settings, fieldInfo, _evaluator, parameterBag)
            .GroupBy(groupingQuery, fieldInfo, _evaluator, parameterBag)
            .Having(groupingQuery, fieldInfo, _evaluator, parameterBag)
            .OrderBy(source, settings, fieldInfo, _evaluator, parameterBag)
            .WithParameters(parameterizedQuery, parameterBag)
            .Build();
    }
}
