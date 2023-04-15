namespace QueryFramework.CodeGeneration.Models;

public interface IQueryParameterValue
{
    [Required]
    string Name { get; }
}
