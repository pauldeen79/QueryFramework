using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Builders
{
    public class QuerySortOrderBuilder : IQuerySortOrderBuilder
    {
        public IQueryExpressionBuilder Field { get; set; }
        public QuerySortOrderDirection Order { get; set; }
        public IQuerySortOrder Build()
        {
            return new QuerySortOrder(Field.Build(), Order);
        }
        public QuerySortOrderBuilder(IQuerySortOrder source = null)
        {
            Field = new QueryExpressionBuilder();
            if (source != null)
            {
                Field.Update(source.Field);
                Order = source.Order;
            }
        }
        public QuerySortOrderBuilder(IQueryExpression expression, QuerySortOrderDirection order = QuerySortOrderDirection.Ascending)
        {
            Field = expression.ToBuilder();
            Order = order;
        }
        public QuerySortOrderBuilder(string fieldName, QuerySortOrderDirection order = QuerySortOrderDirection.Ascending)
        {
            Field = new QueryExpressionBuilder(fieldName);
            Order = order;
        }
    }
}
