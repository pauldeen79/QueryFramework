namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IParameterizedQuery : IQuery
{
    [Required] IReadOnlyCollection<IQueryParameter> Parameters { get; }
}
