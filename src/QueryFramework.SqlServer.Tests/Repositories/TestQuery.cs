namespace QueryFramework.SqlServer.Tests.Repositories;

public record TestQuery : SingleEntityQuery, ITestQuery
{
    public TestQuery()
    {
    }

    public TestQuery(ISingleEntityQuery source) : this(source.Limit, source.Offset, source.Filter, source.OrderByFields)
    {
    }

    public TestQuery(int? limit, int? offset, ComposedEvaluatable filter, IEnumerable<IQuerySortOrder> orderByFields) : base(limit, offset, filter, orderByFields)
    {
    }
}
