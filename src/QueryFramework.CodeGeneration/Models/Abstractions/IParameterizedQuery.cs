namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IParameterizedQuery : IQuery
{
    [Required][ValidateObject] IReadOnlyCollection<IQueryParameter> Parameters { get; }
}
