namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IGroupingQuery : IQuery
{
    [Required] IReadOnlyCollection<Expression> GroupByFields { get; }
    [Required] ComposedEvaluatable GroupByFilter { get; }
}
