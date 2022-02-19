namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluatorProvider
{
    bool TryGetSqlExpression(IExpression expression, ISqlExpressionEvaluator evaluator, out string? result);
}
