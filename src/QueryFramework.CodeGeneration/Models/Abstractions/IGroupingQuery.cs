namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IGroupingQuery : IQuery
{
    [Required][ValidateObject] IReadOnlyCollection<Expression> GroupByFields { get; }
    [Required][ValidateObject] ComposedEvaluatable GroupByFilter { get; }
}
