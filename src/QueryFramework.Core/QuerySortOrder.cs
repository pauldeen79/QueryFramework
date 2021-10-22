using QueryFramework.Abstractions;

namespace QueryFramework.Core
{
    public sealed record QuerySortOrder : IQuerySortOrder
    {
        public QuerySortOrder(string fieldName, QuerySortOrderDirection order = QuerySortOrderDirection.Ascending)
        {
            Field = new QueryExpression(fieldName);
            Order = order;
        }

        public QuerySortOrder(IQueryExpression expression, QuerySortOrderDirection order = QuerySortOrderDirection.Ascending)
        {
            Field = expression;
            Order = order;
        }

        public IQueryExpression Field { get; }

        public QuerySortOrderDirection Order { get; }

        public override string ToString() => $"{Field} {Order}";
    }
}
