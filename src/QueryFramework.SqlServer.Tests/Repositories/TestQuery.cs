namespace QueryFramework.SqlServer.Tests.Repositories;

public record TestQuery : SingleEntityQuery, ITestQuery
{
    public TestQuery()
    {
    }

    public TestQuery(ISingleEntityQuery source) : this(source.Limit, source.Offset, source.Conditions, source.OrderByFields)
    {
    }

    public TestQuery(int? limit, int? offset, IEnumerable<IQueryCondition> conditions, IEnumerable<IQuerySortOrder> orderByFields) : base(limit, offset, conditions, orderByFields)
    {
    }
}
