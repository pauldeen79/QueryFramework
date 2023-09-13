namespace QueryFramework.SqlServer.Tests.TestHelpers;

public class QueryFieldInfoProviderMock : IQueryFieldInfoProvider
{
    public bool ReturnValue { get; set; }
    public Func<IQuery, IQueryFieldInfo?> ResultDelegate { get; set; } = new Func<IQuery, IQueryFieldInfo?>(_ => default(IQueryFieldInfo));

    public bool TryCreate(IQuery query, out IQueryFieldInfo? result)
    {
        result = ResultDelegate.Invoke(query);
        return ReturnValue;
    }
}
