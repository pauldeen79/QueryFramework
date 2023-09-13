namespace QueryFramework.SqlServer.Abstractions;

public interface IQueryFieldInfoFactory
{
    IQueryFieldInfo Create(IQuery query);
}
