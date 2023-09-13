namespace QueryFramework.SqlServer.CrossCutting.Data;

public class QueryPagedDatabaseCommandProvider : IContextPagedDatabaseCommandProvider<IQuery>
{
    private readonly IQueryFieldInfoFactory _fieldInfoFactory;
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders;
    private readonly ISqlExpressionEvaluator _evaluator;

    public QueryPagedDatabaseCommandProvider(IQueryFieldInfoFactory fieldInfoFactory,
                                             IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders,
                                             ISqlExpressionEvaluator evaluator)
    {
        _fieldInfoFactory = fieldInfoFactory;
        _settingsProviders = settingsProviders;
        _evaluator = evaluator;
    }

    public IPagedDatabaseCommand CreatePaged(IQuery source, DatabaseOperation operation, int offset, int pageSize)
        => CreatePaged(source, operation, offset, pageSize, default);

    public IPagedDatabaseCommand CreatePaged(IQuery source, DatabaseOperation operation, int offset, int pageSize, object? context)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        var fieldSelectionQuery = source as IFieldSelectionQuery;
        var groupingQuery = source as IGroupingQuery;
        var parameterizedQuery = source as IParameterizedQuery;
        IPagedDatabaseEntityRetrieverSettings settings;
        try
        {
            settings = (IPagedDatabaseEntityRetrieverSettings)GetType()
                .GetMethod(nameof(Create))
                .MakeGenericMethod(source.GetType())
                .Invoke(this, Array.Empty<object>());
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
        var fieldInfo = _fieldInfoFactory.Create(source);
        var parameterBag = new ParameterBag();
        return new PagedSelectCommandBuilder()
            .Select(settings, fieldInfo, fieldSelectionQuery, _evaluator, parameterBag, context)
            .Top(source, settings)
            .Offset(source)
            .Distinct(fieldSelectionQuery)
            .From(source, settings)
            .Where(source, settings, fieldInfo, _evaluator, parameterBag, context)
            .GroupBy(groupingQuery, fieldInfo, _evaluator, parameterBag, context)
            .Having(groupingQuery, fieldInfo, _evaluator, parameterBag, context)
            .OrderBy(source, settings, fieldInfo, _evaluator, parameterBag, context)
            .WithParameters(parameterizedQuery, parameterBag)
            .Build();
    }

    public IPagedDatabaseEntityRetrieverSettings Create<TResult>() where TResult : class
    {
        foreach (var provider in _settingsProviders)
        {
            var success = provider.TryGet<TResult>(out var result);
            if (success)
            {
                return result ?? throw new InvalidOperationException($"Database entity retriever provider for query type [{typeof(TResult).FullName}] provided an empty result");
            }
        }

        throw new InvalidOperationException($"No database entity retriever provider was found for query type [{typeof(TResult).FullName}]");
    }
}
