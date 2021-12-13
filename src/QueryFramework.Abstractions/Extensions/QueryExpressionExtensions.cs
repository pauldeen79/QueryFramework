namespace QueryFramework.Abstractions.Extensions
{
    public static class QueryExpressionExtensions
    {
        public static string GetExpression(this IQueryExpression instance)
            => instance.Function == null
                    ? instance.FieldName
                    : string.Format(instance.Function.Expression, instance.FieldName);
    }
}
