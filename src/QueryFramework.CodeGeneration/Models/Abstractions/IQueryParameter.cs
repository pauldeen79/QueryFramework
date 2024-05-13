namespace QueryFramework.CodeGeneration.Models.Abstractions;

internal interface IQueryParameter
{
    [Required] string Name { get; }
    object? Value { get; }
}
