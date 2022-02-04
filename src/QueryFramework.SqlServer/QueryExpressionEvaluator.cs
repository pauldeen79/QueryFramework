namespace QueryFramework.SqlServer;

public class QueryExpressionEvaluator : IQueryExpressionEvaluator
{
    private readonly IEnumerable<IFunctionParser> _functionParsers;

    public QueryExpressionEvaluator(IEnumerable<IFunctionParser> functionParsers)
        => _functionParsers = functionParsers;

    public string GetSqlExpression(IQueryExpression expression)
        => expression.Function == null
            ? expression.FieldName
            : GetSqlExpression(expression.Function, _functionParsers).Replace("{0}", expression.FieldName);

    private string GetSqlExpression(IQueryExpressionFunction function, IEnumerable<IFunctionParser> functionParsers)
    {
        var inner = function.InnerFunction != null
            ? GetSqlExpression(function.InnerFunction, functionParsers)
            : string.Empty;

        foreach (var parser in functionParsers)
        {
            if (parser.TryParse(function, this, out var sqlExpression))
            {
                return Combine(sqlExpression, inner);
            }
        }

        throw new ArgumentException($"Unsupported function: {function.GetType().Name}", nameof(function));
    }

    private static string Combine(string sqlExpression, string inner)
        => inner.Length > 0
            ? string.Format(sqlExpression, inner)
            : sqlExpression;
}
