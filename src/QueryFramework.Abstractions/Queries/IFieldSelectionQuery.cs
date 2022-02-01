namespace QueryFramework.Abstractions.Queries;

public interface IFieldSelectionQuery : ISingleEntityQuery
{
    bool Distinct { get; }
    bool GetAllFields { get; }
    ValueCollection<IQueryExpression> Fields { get; }
}
