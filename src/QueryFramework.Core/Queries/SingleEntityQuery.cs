namespace QueryFramework.Core.Queries;

public record SingleEntityQuery : ISingleEntityQuery
{
    public SingleEntityQuery() : this(null,
                                      null,
                                      Enumerable.Empty<ICondition>(),
                                      Enumerable.Empty<IQuerySortOrder>())
    {
    }

    public SingleEntityQuery(int? limit,
                             int? offset,
                             IEnumerable<ICondition> conditions,
                             IEnumerable<IQuerySortOrder> orderByFields)
    {
        Limit = limit;
        Offset = offset;
        Conditions = new ValueCollection<ICondition>(conditions);
        OrderByFields = new ValueCollection<IQuerySortOrder>(orderByFields);
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public int? Limit { get; }
    public int? Offset { get; }
    public IReadOnlyCollection<ICondition> Conditions { get; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}
