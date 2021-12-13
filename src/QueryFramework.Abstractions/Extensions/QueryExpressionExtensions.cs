namespace QueryFramework.Abstractions.Extensions
{
    public static class QueryExpressionExtensions
    {
        public static string GetExpression(this IQueryExpression instance, string expression)
            => instance.Function == null
                    ? instance.FieldName
                    : string.Format(expression, instance.FieldName);
    }
}
