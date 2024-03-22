namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IQuery
{
    int? Limit { get; }
    int? Offset { get; }
    [Required][ValidateObject] ComposedEvaluatable Filter { get; }
    [Required][ValidateObject] IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}
