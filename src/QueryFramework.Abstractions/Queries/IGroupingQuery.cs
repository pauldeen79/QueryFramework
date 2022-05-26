namespace QueryFramework.Abstractions.Queries;

public interface IGroupingQuery : ISingleEntityQuery
{
    IReadOnlyCollection<IExpression> GroupByFields { get; }
    IReadOnlyCollection<ICondition> HavingFields { get; }
}
