namespace QueryFramework.SqlServer;

public class DefaultSqlExpressionEvaluator : ISqlExpressionEvaluator
{
    private readonly IEnumerable<ISqlExpressionEvaluatorProvider> _sqlExpressionEvaluatorProviders;

    public DefaultSqlExpressionEvaluator(IEnumerable<ISqlExpressionEvaluatorProvider> sqlExpressionEvaluatorProviders)
        => _sqlExpressionEvaluatorProviders = sqlExpressionEvaluatorProviders;

    public string GetSqlExpression(IExpression expression)
    {
        foreach (var sqlExpressionEvaluatorProvider in _sqlExpressionEvaluatorProviders)
        {
            if (sqlExpressionEvaluatorProvider.TryGetSqlExpression(expression, this, out var result))
            {
                return result ?? string.Empty;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
    }
}
