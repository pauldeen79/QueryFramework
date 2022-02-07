namespace QueryFramework.SqlServer.Tests.TestHelpers;

public class QueryFieldInfoProviderMock : IQueryFieldInfoProvider
{
    public bool ReturnValue { get; set; }
    public Func<ISingleEntityQuery, IQueryFieldInfo?> ResultDelegate { get; set; } = new Func<ISingleEntityQuery, IQueryFieldInfo?>(_ => default(IQueryFieldInfo));

    public bool TryCreate(ISingleEntityQuery query, out IQueryFieldInfo? result)
    {
        result = ResultDelegate.Invoke(query);
        return ReturnValue;
    }
}
