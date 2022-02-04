namespace QueryFramework.SqlServer;

public class DefaultQueryExpressionEvaluator : IQueryExpressionEvaluator
{
    private readonly IEnumerable<IFunctionParser> _functionParsers;

    public DefaultQueryExpressionEvaluator(IEnumerable<IFunctionParser> functionParsers)
        => _functionParsers = functionParsers;

    public string GetSqlExpression(IQueryExpression expression)
        => expression.Function == null
            ? expression.FieldName
            : GetSqlExpression(expression.Function, _functionParsers, nameof(expression)).Replace("{0}", expression.FieldName);

    private string GetSqlExpression(IQueryExpressionFunction function,
                                    IEnumerable<IFunctionParser> functionParsers,
                                    string paramName)
    {
        var inner = function.InnerFunction != null
            ? GetSqlExpression(function.InnerFunction, functionParsers, paramName)
            : string.Empty;

        foreach (var parser in functionParsers)
        {
            if (parser.TryParse(function, this, out var sqlExpression))
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
