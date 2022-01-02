using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions
{
    public static class QuerySortOrderBuilderExtensions
    {
        public static IQuerySortOrderBuilder WithField(this IQuerySortOrderBuilder instance, IQueryExpressionBuilder field)
        {
            instance.Field = field;
            return instance;
        }
        public static IQuerySortOrderBuilder WithField(this IQuerySortOrderBuilder instance, string fieldName)
        {
            instance.Field.FieldName = fieldName;
            return instance;
        }
        public static IQuerySortOrderBuilder WithOrder(this IQuerySortOrderBuilder instance, QuerySortOrderDirection order)
        {
            instance.Order = order;
            return instance;
        }
    }
}
