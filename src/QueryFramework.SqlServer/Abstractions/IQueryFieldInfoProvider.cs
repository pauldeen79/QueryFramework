namespace QueryFramework.SqlServer.Abstractions;

public interface IQueryFieldInfoProvider
{
    bool TryCreate(IQuery query, out IQueryFieldInfo? result);
}
