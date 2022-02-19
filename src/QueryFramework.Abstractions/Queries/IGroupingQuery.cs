namespace QueryFramework.Abstractions.Queries;

public interface IGroupingQuery : ISingleEntityQuery
{
    ValueCollection<IExpression> GroupByFields { get; }
    ValueCollection<ICondition> HavingFields { get; }
}
