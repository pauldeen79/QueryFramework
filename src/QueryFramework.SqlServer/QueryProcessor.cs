namespace QueryFramework.SqlServer;

public class QueryProcessor<TQuery, TResult> : IQueryProcessor<TQuery, TResult>
    where TQuery : ISingleEntityQuery
    where TResult : class
{
    private readonly IDatabaseEntityRetriever<TResult> _retriever;
    private readonly IPagedDatabaseCommandProvider<TQuery> _databaseCommandProvider;

    public QueryProcessor(IDatabaseEntityRetriever<TResult> retriever,
                          IPagedDatabaseCommandProvider<TQuery> databaseCommandProvider)
    {
        _retriever = retriever;
        _databaseCommandProvider = databaseCommandProvider;
    }

    public IReadOnlyCollection<TResult> FindMany(TQuery query)
        => _retriever.FindMany(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand);

    public TResult? FindOne(TQuery query)
        => _retriever.FindOne(GenerateCommand(query, 1).DataCommand);

    public IPagedResult<TResult> FindPaged(TQuery query)
        => _retriever.FindPaged(GenerateCommand(query, query.Limit.GetValueOrDefault()));

    private IPagedDatabaseCommand GenerateCommand(TQuery query, int limit)
        => _databaseCommandProvider.CreatePaged
        (
            query.Validate(),
            DatabaseOperation.Select,
            query.Offset.GetValueOrDefault(),
            limit
        );
}
