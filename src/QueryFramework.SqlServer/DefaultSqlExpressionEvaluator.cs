namespace QueryFramework.SqlServer;

public class DefaultSqlExpressionEvaluator : ISqlExpressionEvaluator
{
    private readonly IEnumerable<IFunctionParser> _functionParsers;

    public DefaultSqlExpressionEvaluator(IEnumerable<IFunctionParser> functionParsers)
        => _functionParsers = functionParsers;

    public string GetSqlExpression(IExpression expression)
        => expression.Function == null
            ? expression.GetFieldName() ?? throw new ArgumentException("Expression contains no field name", nameof(expression))
            : GetSqlExpression(expression.Function, _functionParsers, nameof(expression)).Replace("{0}", expression.GetFieldName() ?? throw new ArgumentException("Expression contains no field name", nameof(expression)));

    private string GetSqlExpression(IExpressionFunction function,
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
