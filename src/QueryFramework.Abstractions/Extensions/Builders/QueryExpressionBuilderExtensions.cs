using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions.Builders
{
    public static class QueryExpressionBuilderExtensions
    {
        public static IQueryExpressionBuilder Clear(this IQueryExpressionBuilder instance)
        {
            instance.FieldName = string.Empty;
            instance.Expression = null;
            return instance;
        }
        public static IQueryExpressionBuilder Update(this IQueryExpressionBuilder instance, IQueryExpression source)
        {
            instance.FieldName = string.Empty;
            instance.Expression = null;
            if (source != null)
            {
                instance.FieldName = source.FieldName;
                instance.Expression = !(source is IExpressionContainer expressionContainer)
                    ? source.Expression
                    : expressionContainer.SourceExpression;
            }
            return instance;
        }
        public static IQueryExpressionBuilder WithFieldName(this IQueryExpressionBuilder instance, string fieldName)
        {
            instance.FieldName = fieldName;
            return instance;
        }
        public static IQueryExpressionBuilder WithExpression(this IQueryExpressionBuilder instance, string? expression)
        {
            instance.Expression = expression;
            return instance;
        }
    }
}
