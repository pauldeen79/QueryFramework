namespace QueryFramework.CodeGeneration.Models.Abstractions;

internal interface IGroupingQuery : IQuery
{
    [Required][ValidateObject] IReadOnlyCollection<IExpression> GroupByFields { get; }
    [Required][ValidateObject][ValidGroups] IReadOnlyCollection<ICondition> GroupByFilter { get; }
}
