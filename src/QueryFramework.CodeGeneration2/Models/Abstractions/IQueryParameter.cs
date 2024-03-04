namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IQueryParameter
{
    [Required] string Name { get; }
    object Value { get; }
}
