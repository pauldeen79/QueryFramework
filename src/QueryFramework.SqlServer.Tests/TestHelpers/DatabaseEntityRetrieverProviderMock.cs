namespace QueryFramework.SqlServer.Tests.TestHelpers;

public class DatabaseEntityRetrieverProviderMock<T> : IDatabaseEntityRetrieverProvider where T : class
{
    public bool ReturnValue { get; set; }
    public Func<IQuery, IDatabaseEntityRetriever<T>?> ResultDelegate { get; set; }
        = new Func<IQuery, IDatabaseEntityRetriever<T>?>(_ => default(IDatabaseEntityRetriever<T>));

    public bool TryCreate<TResult>(IQuery query, out IDatabaseEntityRetriever<TResult>? result) where TResult : class
    {
        result = ResultDelegate.Invoke(query) as IDatabaseEntityRetriever<TResult>;
        return ReturnValue;
    }
}
