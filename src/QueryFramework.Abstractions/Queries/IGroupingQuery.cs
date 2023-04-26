namespace QueryFramework.Abstractions.Queries;

public interface IGroupingQuery : ISingleEntityQuery
{
    IReadOnlyCollection<Expression> GroupByFields { get; }
    ComposedEvaluatable GroupByFilter { get; }
}
