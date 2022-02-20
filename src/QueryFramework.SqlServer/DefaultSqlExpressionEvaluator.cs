namespace QueryFramework.SqlServer;

public class DefaultSqlExpressionEvaluator : ISqlExpressionEvaluator
{
    private readonly IEnumerable<ISqlExpressionEvaluatorProvider> _sqlExpressionEvaluatorProviders;
    private readonly IEnumerable<IFunctionParser> _functionParsers;

    public DefaultSqlExpressionEvaluator(IEnumerable<ISqlExpressionEvaluatorProvider> sqlExpressionEvaluatorProviders,
                                         IEnumerable<IFunctionParser> functionParsers)
    {
        _sqlExpressionEvaluatorProviders = sqlExpressionEvaluatorProviders;
        _functionParsers = functionParsers;
    }

    public string GetSqlExpression(IExpression expression, IQueryFieldInfo fieldInfo, int paramCounter)
    {
        string? result = null;
        foreach (var sqlExpressionEvaluatorProvider in _sqlExpressionEvaluatorProviders)
        {
            if (sqlExpressionEvaluatorProvider.TryGetSqlExpression(expression, this, fieldInfo, paramCounter, out var providerResult))
            {
                result = providerResult ?? string.Empty;
                break;
            }
        }

        if (result == null)
        {
            throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
        }

        return expression.Function == null
            ? result
            : GetSqlExpression(expression.Function, nameof(expression)).Replace("{0}", result);
    }

    public string GetLengthExpression(IExpression expression, IQueryFieldInfo fieldInfo)
    {
        foreach (var sqlExpressionEvaluatorProvider in _sqlExpressionEvaluatorProviders)
        {
            if (sqlExpressionEvaluatorProvider.TryGetLengthExpression(expression, this, fieldInfo, out var providerResult))
            {
                return providerResult ?? string.Empty;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
    }

    private string GetSqlExpression(IExpressionFunction function, string paramName)
    {
        var inner = function.InnerFunction != null
            ? GetSqlExpression(function.InnerFunction, paramName)
            : string.Empty;

        foreach (var parser in _functionParsers)
        {
            if (parser.TryParse(function, this, out var sqlExpression))
            {
                return Combine(sqlExpression, inner);
            }
        }

        throw new ArgumentOutOfRangeException(paramName, $"Unsupported function: {function.GetType().Name}");
    }

    private static string Combine(string sqlExpression, string inner)
        => inner.Length > 0
            ? string.Format(sqlExpression, inner)
            : sqlExpression;
}
