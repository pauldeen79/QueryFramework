namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IQuery
{
    int? Limit { get; }
    int? Offset { get; }
    [Required] ComposedEvaluatable Filter { get; }
    [Required] IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}
