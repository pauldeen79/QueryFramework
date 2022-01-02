using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;

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
        public QuerySortOrderBuilder()
        {
            Field = new QueryExpressionBuilder();
        }
        public QuerySortOrderBuilder(IQuerySortOrder source)
        {
            Field = new QueryExpressionBuilder();
            Field.FieldName = source.Field.FieldName;
            Field.Function = source.Field.Function;
            Order = source.Order;
        }
        public QuerySortOrderBuilder(IQueryExpression expression) : this(expression, QuerySortOrderDirection.Ascending)
        {
        }
        public QuerySortOrderBuilder(IQueryExpression expression, QuerySortOrderDirection order) : this(expression.FieldName, expression.Function, order)
        {
        }
        public QuerySortOrderBuilder(string fieldName) : this(fieldName, QuerySortOrderDirection.Ascending)
        {
        }
        public QuerySortOrderBuilder(string fieldName, QuerySortOrderDirection order) : this(fieldName, null, order)
        {
        }
        public QuerySortOrderBuilder(string fieldName, IQueryExpressionFunction? function, QuerySortOrderDirection order)
        {
            Field = new QueryExpressionBuilder(fieldName, function);
            Order = order;
        }
    }
}
