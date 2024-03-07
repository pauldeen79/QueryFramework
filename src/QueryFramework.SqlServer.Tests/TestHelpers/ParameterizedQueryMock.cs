namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal sealed class ParameterizedQueryMock : IParameterizedQuery
{
    public ParameterizedQueryMock(IEnumerable<IQueryParameter> parameters)
    {
        Filter = new(Enumerable.Empty<ComposableEvaluatable>());
        OrderByFields = new ReadOnlyValueCollection<IQuerySortOrder>();
        Parameters = new ReadOnlyValueCollection<IQueryParameter>(parameters.ToList());
    }

    public IReadOnlyCollection<IQueryParameter> Parameters { get; set; }
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public ComposedEvaluatable Filter { get; set; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

    public IQueryBuilder ToBuilder()
    {
        throw new NotImplementedException();
    }
}
