namespace QueryFramework.SqlServer.Abstractions;

public interface IQueryFieldInfoProvider
{
    bool TryCreate(ISingleEntityQuery query, out IQueryFieldInfo? result);
}
