namespace QueryFramework.SqlServer;

/// <summary>
/// Provider for DefaultQueryFieldInfo.
/// </summary>
public class DefaultQueryFieldInfoProvider : IQueryFieldInfoProvider
{
    public bool TryCreate(ISingleEntityQuery query, out IQueryFieldInfo? result)
    {
        result = new DefaultQueryFieldInfo();
        return true;
    }
}
