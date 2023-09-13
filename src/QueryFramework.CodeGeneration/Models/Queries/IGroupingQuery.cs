namespace QueryFramework.CodeGeneration.Models.Queries;

public interface IGroupingQuery : IQuery
{
    IReadOnlyCollection<Expression> GroupByFields { get; }
    ComposedEvaluatable GroupByFilter { get; }
}
