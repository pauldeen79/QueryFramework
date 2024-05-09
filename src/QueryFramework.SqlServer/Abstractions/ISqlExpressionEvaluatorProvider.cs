namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluatorProvider
{
    bool TryGetSqlExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, out string? result);
    bool TryGetLengthExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result);
}
