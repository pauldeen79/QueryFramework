namespace QueryFramework.CodeGeneration.Models.Abstractions;

internal interface IQueryParameterValue
{
    [Required] string Name { get; }
}
