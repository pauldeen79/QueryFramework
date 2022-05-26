namespace QueryFramework.Abstractions.Queries;

public interface IParameterizedQuery : ISingleEntityQuery
{
    IReadOnlyCollection<IQueryParameter> Parameters { get; }
}
