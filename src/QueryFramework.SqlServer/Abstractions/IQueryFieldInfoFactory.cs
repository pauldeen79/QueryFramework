namespace QueryFramework.SqlServer.Abstractions;

public interface IQueryFieldInfoFactory
{
    IQueryFieldInfo Create(ISingleEntityQuery query);
}
