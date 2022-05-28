namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal class ParameterizedQueryMock : IParameterizedQuery
{
    public ParameterizedQueryMock(IEnumerable<IQueryParameter> parameters)
    {
        Conditions = new ReadOnlyValueCollection<ICondition>();
        OrderByFields = new ReadOnlyValueCollection<IQuerySortOrder>();
        Parameters = new ReadOnlyValueCollection<IQueryParameter>(parameters.ToList());
    }

    public IReadOnlyCollection<IQueryParameter> Parameters { get; set; }
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public IReadOnlyCollection<ICondition> Conditions { get; set; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }
}
