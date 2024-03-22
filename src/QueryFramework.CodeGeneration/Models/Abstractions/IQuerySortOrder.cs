namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IQuerySortOrder
{
    [Required][ValidateObject] Expression FieldNameExpression { get; }
    QuerySortOrderDirection Order { get; }
}
