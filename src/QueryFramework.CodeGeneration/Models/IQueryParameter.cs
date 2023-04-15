namespace QueryFramework.CodeGeneration.Models;

public interface IQueryParameter
{
    [Required]
    string Name { get; }

    object Value { get;}
}
