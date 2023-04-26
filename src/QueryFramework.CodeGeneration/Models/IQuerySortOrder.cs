namespace QueryFramework.CodeGeneration.Models;

public interface IQuerySortOrder
{
    [Required]
    string FieldName { get; }

    QuerySortOrderDirection Order { get; }
}
