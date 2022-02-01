namespace QueryFramework.SqlServer;

public class QueryProcessor<TQuery, TResult> : IQueryProcessor<TQuery, TResult>
    where TQuery : ISingleEntityQuery
    where TResult : class
{
    private IDatabaseEntityRetriever<TResult> Retriever { get; }
    private IPagedDatabaseCommandProvider<TQuery> DatabaseCommandProvider { get; }

    public QueryProcessor(IDatabaseEntityRetriever<TResult> retriever,
                          IPagedDatabaseCommandProvider<TQuery> databaseCommandProvider)
    {
        Retriever = retriever;
        DatabaseCommandProvider = databaseCommandProvider;
    }

    public IReadOnlyCollection<TResult> FindMany(TQuery query)
        => Retriever.FindMany(GenerateCommand(query, query.Limit.GetValueOrDefault()).DataCommand);

    public TResult? FindOne(TQuery query)
        => Retriever.FindOne(GenerateCommand(query, 1).DataCommand);

    public IPagedResult<TResult> FindPaged(TQuery query)
        => Retriever.FindPaged(GenerateCommand(query, query.Limit.GetValueOrDefault()));

    private IPagedDatabaseCommand GenerateCommand(TQuery query, int limit)
        => DatabaseCommandProvider.CreatePaged
        (
            query.Validate(),
            DatabaseOperation.Select,
            query.Offset.GetValueOrDefault(),
            limit
        );
}
