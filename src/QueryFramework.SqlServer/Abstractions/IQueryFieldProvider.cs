namespace QueryFramework.SqlServer.Abstractions;

public interface IQueryFieldProvider
{
    string? GetDatabaseFieldName(string queryFieldName);
    IEnumerable<string> GetAllFields();
}
