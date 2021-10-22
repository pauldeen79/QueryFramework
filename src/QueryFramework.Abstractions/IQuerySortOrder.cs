namespace QueryFramework.Abstractions
{
    public interface IQuerySortOrder
    {
        IQueryExpression Field { get; }

        QuerySortOrderDirection Order { get; }
    }
}
