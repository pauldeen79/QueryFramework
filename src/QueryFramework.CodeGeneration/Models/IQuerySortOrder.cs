namespace QueryFramework.CodeGeneration.Models;

public interface IQuerySortOrder
{
    [Required]
    ITypedExpression<string> FieldNameExpression { get; }

    QuerySortOrderDirection Order { get; }
}
