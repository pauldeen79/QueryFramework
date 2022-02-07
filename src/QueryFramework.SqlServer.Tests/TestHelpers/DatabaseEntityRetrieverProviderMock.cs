namespace QueryFramework.SqlServer.Tests.TestHelpers;

public class DatabaseEntityRetrieverProviderMock<T> : IDatabaseEntityRetrieverProvider where T : class
{
    public bool ReturnValue { get; set; }
    public Func<ISingleEntityQuery, IDatabaseEntityRetriever<T>?> ResultDelegate { get; set; }
        = new Func<ISingleEntityQuery, IDatabaseEntityRetriever<T>?>(_ => default(IDatabaseEntityRetriever<T>));

    public bool TryCreate<TResult>(ISingleEntityQuery query, out IDatabaseEntityRetriever<TResult>? result) where TResult : class
    {
        result = ResultDelegate.Invoke(query) as IDatabaseEntityRetriever<TResult>;
        return ReturnValue;
    }
}
