namespace QueryFramework.Core.Queries;

public record FieldSelectionQuery : IFieldSelectionQuery, IValidatableObject
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
        Fields = new ValueCollection<IExpression>(fields);
        Conditions = new ValueCollection<ICondition>(conditions);
        OrderByFields = new ValueCollection<IQuerySortOrder>(orderByFields);
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public int? Limit { get; }
    public int? Offset { get; }
    public bool Distinct { get; }
    public bool GetAllFields { get; }
    public ValueCollection<IExpression> Fields { get; }
    public ValueCollection<ICondition> Conditions { get; }
    public ValueCollection<IQuerySortOrder> OrderByFields { get; }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => this.ValidateQuery();
}
