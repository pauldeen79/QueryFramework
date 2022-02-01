namespace QueryFramework.SqlServer.Extensions;

public static class QueryExpressionExtensions
{
    public static string GetSqlExpression(this IQueryExpression expression)
        => expression.Function == null
            ? expression.FieldName
            : expression.Function.GetSqlExpression().Replace("{0}", expression.FieldName);
}
