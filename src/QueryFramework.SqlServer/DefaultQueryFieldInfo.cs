namespace QueryFramework.SqlServer;

/// <summary>
/// Default query field info, which has enough functionality for single-entity queries. Getting all select fields (SelectAll) is not supported.
/// </summary>
public class DefaultQueryFieldInfo : IQueryFieldInfo
{
    public IEnumerable<string> GetAllFields()
        => Enumerable.Empty<string>();

    public string? GetDatabaseFieldName(string queryFieldName)
        => queryFieldName;
}
