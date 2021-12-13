using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QueryExpressionFunctionExtensions
    {
        public static string GetExpression(this IQueryExpressionFunction? innerFunction, string expression)
            => innerFunction == null
                ? expression
                : expression.Replace("{0}", innerFunction.Expression);
    }
}
