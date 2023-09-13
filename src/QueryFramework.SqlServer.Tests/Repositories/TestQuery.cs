namespace QueryFramework.SqlServer.Tests.Repositories;

public record TestQuery : SingleEntityQuery, ITestQuery
{
    public TestQuery() : base(null, null, new ComposedEvaluatableBuilder().BuildTyped(), Enumerable.Empty<IQuerySortOrder>())
    {
    }

    public TestQuery(IQuery source) : this(source.Limit, source.Offset, source.Filter, source.OrderByFields)
    {
    }

    public TestQuery(int? limit, int? offset, ComposedEvaluatable filter, IEnumerable<IQuerySortOrder> orderByFields) : base(limit, offset, filter, orderByFields)
    {
    }
}
