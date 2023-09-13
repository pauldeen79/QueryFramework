namespace QueryFramework.CodeGeneration.Models;

public interface IQuery
{
    int? Limit { get; }
    int? Offset { get; }
    ComposedEvaluatable Filter { get; }
    IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}
