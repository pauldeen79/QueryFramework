namespace QueryFramework.CodeGeneration.Models;

public interface IQuerySortOrder
{
    [Required]
    Expression FieldNameExpression { get; }

    QuerySortOrderDirection Order { get; }
}
