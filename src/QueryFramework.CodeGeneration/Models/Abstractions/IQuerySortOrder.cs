namespace QueryFramework.CodeGeneration.Models.Abstractions;

internal interface IQuerySortOrder
{
    [Required][ValidateObject] IExpression<string> FieldNameExpression { get; }
    QuerySortOrderDirection Order { get; }
}
