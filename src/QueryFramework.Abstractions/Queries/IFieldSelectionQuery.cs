namespace QueryFramework.Abstractions.Queries;

public interface IFieldSelectionQuery : ISingleEntityQuery
{
    bool Distinct { get; }
    bool GetAllFields { get; }
    IReadOnlyCollection<Expression> Fields { get; }
}
