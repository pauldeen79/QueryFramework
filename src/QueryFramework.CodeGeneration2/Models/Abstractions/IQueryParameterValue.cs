namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IQueryParameterValue
{
    [Required] string Name { get; }
}
