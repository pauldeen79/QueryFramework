namespace QueryFramework.SqlServer.Extensions;

public static class ExpressionFunctionExtensions
{
    public static string GetSqlExpression(this IExpressionFunction function,
                                          ISqlExpressionEvaluator evaluator,
                                          IEnumerable<IFunctionParser> functionParsers,
                                          string paramName)
    {
        var inner = function.InnerFunction != null
            ? function.InnerFunction.GetSqlExpression(evaluator, functionParsers, paramName)
            : string.Empty;

        foreach (var parser in functionParsers)
        {
            if (parser.TryParse(function, evaluator, out var sqlExpression))
            {
                return Combine(sqlExpression, inner);
            }
        }

        throw new ArgumentException($"Unsupported function: {function.GetType().Name}", paramName);
    }

    private static string Combine(string sqlExpression, string inner)
        => inner.Length > 0
            ? string.Format(sqlExpression, inner)
            : sqlExpression;
}
