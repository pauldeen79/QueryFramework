namespace QueryFramework.CodeGeneration.Models.Queries;

public interface IFieldSelectionQuery : IQuery
{
    bool Distinct { get; }
    bool GetAllFields { get; }
    IReadOnlyCollection<string> FieldNames { get; }
}
