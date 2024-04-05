namespace QueryFramework.CodeGeneration.Models.Abstractions;

internal interface IQuerySortOrder
{
    [Required][ValidateObject] Expression FieldNameExpression { get; }
    QuerySortOrderDirection Order { get; }
}
