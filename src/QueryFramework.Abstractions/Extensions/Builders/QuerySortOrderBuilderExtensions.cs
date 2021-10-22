using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions.Builders
{
    public static class QuerySortOrderBuilderExtensions
    {
        public static IQuerySortOrderBuilder Clear(this IQuerySortOrderBuilder instance)
        {
            instance.Field.Clear();
            instance.Order = default;
            return instance;
        }
        public static IQuerySortOrderBuilder Update(this IQuerySortOrderBuilder instance, IQuerySortOrder source)
        {
            instance.Field.Clear();
            instance.Order = default;
            if (source != null)
            {
                instance.Field.Update(source.Field);
                instance.Order = source.Order;
            }
            return instance;
        }
        public static IQuerySortOrderBuilder WithField(this IQuerySortOrderBuilder instance, IQueryExpressionBuilder field)
        {
            instance.Field = field;
            return instance;
        }
        public static IQuerySortOrderBuilder WithField(this IQuerySortOrderBuilder instance, IQueryExpression field)
        {
            instance.Field.Update(field);
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
