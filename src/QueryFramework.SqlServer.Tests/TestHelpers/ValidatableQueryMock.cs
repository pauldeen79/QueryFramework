namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal sealed class ValidatableQueryMock : IQuery, IValidatableObject
{
    public int? Limit { get; set; }

    public int? Offset { get; set; }

    public ComposedEvaluatable Filter{ get; set; }

    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Limit is null)
        {
            yield return new ValidationResult("Limit is required", new[] { nameof(Limit) });
        }
        if (Offset is null)
        {
            yield return new ValidationResult("Offset is required", new[] { nameof(Offset) });
        }
    }

    public ValidatableQueryMock()
    {
        Filter = new(Enumerable.Empty<ComposableEvaluatable>());
        OrderByFields = new ReadOnlyValueCollection<IQuerySortOrder>();
    }
}
