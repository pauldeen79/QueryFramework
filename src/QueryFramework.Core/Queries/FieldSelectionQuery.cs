namespace QueryFramework.Core.Queries;

public record FieldSelectionQuery : IFieldSelectionQuery
{
    public FieldSelectionQuery() : this(null,
                                        null,
                                        false,
                                        false,
                                        Enumerable.Empty<ICondition>(),
                                        Enumerable.Empty<IQuerySortOrder>(),
                                        Enumerable.Empty<IExpression>())
    {
    }

    public FieldSelectionQuery(int? limit,
                               int? offset,
                               bool distinct,
                               bool getAllFields,
                               IEnumerable<ICondition> conditions,
                               IEnumerable<IQuerySortOrder> orderByFields,
                               IEnumerable<IExpression> fields)
    {
        Limit = limit;
        Offset = offset;
        Distinct = distinct;
        GetAllFields = getAllFields;
        Fields = new ReadOnlyValueCollection<IExpression>(fields);
        Conditions = new ReadOnlyValueCollection<ICondition>(conditions);
        OrderByFields = new ReadOnlyValueCollection<IQuerySortOrder>(orderByFields);
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public int? Limit { get; }
    public int? Offset { get; }
    public bool Distinct { get; }
    public bool GetAllFields { get; }
    public IReadOnlyCollection<IExpression> Fields { get; }
    public IReadOnlyCollection<ICondition> Conditions { get; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}
