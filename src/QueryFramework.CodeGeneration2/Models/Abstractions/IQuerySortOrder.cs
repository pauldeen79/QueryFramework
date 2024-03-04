namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IQuerySortOrder
{
    [Required] Expression FieldNameExpression { get; }
    QuerySortOrderDirection Order { get; }
}
