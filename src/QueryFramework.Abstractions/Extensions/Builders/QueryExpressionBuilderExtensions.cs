using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions.Builders
{
    public static class QueryExpressionBuilderExtensions
    {
        public static IQueryExpressionBuilder Clear(this IQueryExpressionBuilder instance)
        {
            instance.FieldName = string.Empty;
            instance.Function = null;
            return instance;
        }
        public static IQueryExpressionBuilder Update(this IQueryExpressionBuilder instance, IQueryExpression source)
        {
            instance.FieldName = string.Empty;
            instance.Function = null;
            if (source != null)
            {
                instance.FieldName = source.FieldName;
                instance.Function = source.Function;
            }
            return instance;
        }
        public static IQueryExpressionBuilder WithFieldName(this IQueryExpressionBuilder instance, string fieldName)
        {
            instance.FieldName = fieldName;
            return instance;
        }
        public static IQueryExpressionBuilder WithFunction(this IQueryExpressionBuilder instance, IQueryExpressionFunction? function)
        {
            instance.Function = function;
            return instance;
        }
    }
}
