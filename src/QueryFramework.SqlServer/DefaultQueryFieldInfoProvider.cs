namespace QueryFramework.SqlServer;

/// <summary>
/// Provider for DefaultQueryFieldInfo.
/// </summary>
public class DefaultQueryFieldInfoProvider : IQueryFieldInfoProvider
{
    public bool TryCreate(IQuery query, out IQueryFieldInfo? result)
    {
        result = new DefaultQueryFieldInfo();
        return true;
    }
}
