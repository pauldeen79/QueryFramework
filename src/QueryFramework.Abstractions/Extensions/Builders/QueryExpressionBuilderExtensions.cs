using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions.Builders
{
    public static class QueryExpressionBuilderExtensions
    {
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
