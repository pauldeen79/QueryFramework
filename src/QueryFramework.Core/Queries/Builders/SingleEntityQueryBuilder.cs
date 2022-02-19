namespace QueryFramework.Core.Queries.Builders;

public sealed class SingleEntityQueryBuilder : ISingleEntityQueryBuilder
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public List<IConditionBuilder> Conditions { get; set; }
    public List<IQuerySortOrderBuilder> OrderByFields { get; set; }

    public SingleEntityQueryBuilder()
    {
        Conditions = new List<IConditionBuilder>();
        OrderByFields = new List<IQuerySortOrderBuilder>();
    }

    public ISingleEntityQuery Build()
        => new SingleEntityQuery(Limit,
                                 Offset,
                                 Conditions.Select(x => x.Build()),
                                 OrderByFields.Select(x => x.Build()));
}
