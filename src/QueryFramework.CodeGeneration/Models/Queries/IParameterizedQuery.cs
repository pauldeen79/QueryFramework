namespace QueryFramework.CodeGeneration.Models.Queries;

public interface IParameterizedQuery : IQuery
{
    IReadOnlyCollection<IQueryParameter> Parameters { get; }
}
