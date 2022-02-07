namespace QueryFramework.SqlServer.Abstractions;

public interface IQueryFieldInfo
{
    string? GetDatabaseFieldName(string queryFieldName);
    IEnumerable<string> GetAllFields();
}
