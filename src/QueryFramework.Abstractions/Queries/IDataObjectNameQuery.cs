namespace QueryFramework.Abstractions.Queries;

public interface IDataObjectNameQuery : ISingleEntityQuery
{
    string DataObjectName { get; }
}
