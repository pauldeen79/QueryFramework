namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal class ParameterizedQueryMock : IParameterizedQuery
{
    public ParameterizedQueryMock(IEnumerable<IQueryParameter> parameters)
    {
        Conditions = new ValueCollection<IQueryCondition>();
        OrderByFields = new ValueCollection<IQuerySortOrder>();
        Parameters = new ValueCollection<IQueryParameter>(parameters.ToList());
    }

    public ValueCollection<IQueryParameter> Parameters { get; set; }
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public ValueCollection<IQueryCondition> Conditions { get; set; }
    public ValueCollection<IQuerySortOrder> OrderByFields { get; set; }
}
