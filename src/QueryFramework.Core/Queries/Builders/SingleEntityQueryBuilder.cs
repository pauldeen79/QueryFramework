namespace QueryFramework.Core.Queries.Builders;

public sealed class SingleEntityQueryBuilder : ISingleEntityQueryBuilder
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public ComposedEvaluatableBuilder Filter { get; set; }
    public List<IQuerySortOrderBuilder> OrderByFields { get; set; }

    public SingleEntityQueryBuilder()
    {
        Filter = new();
        OrderByFields = new();
    }

    public ISingleEntityQuery Build()
        => new SingleEntityQuery(Limit,
                                 Offset,
                                 Filter.BuildTyped(),
                                 OrderByFields.Select(x => x.Build()));
}
