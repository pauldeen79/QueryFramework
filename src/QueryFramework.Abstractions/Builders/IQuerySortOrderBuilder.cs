namespace QueryFramework.Abstractions.Builders
{
    public interface IQuerySortOrderBuilder
    {
        IQueryExpressionBuilder Field { get; set; }
        QuerySortOrderDirection Order { get; set; }

        IQuerySortOrder Build();
    }
}