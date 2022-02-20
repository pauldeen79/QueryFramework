namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluatorProvider
{
    bool TryGetSqlExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, int paramCounter, out string? result);
    bool TryGetLengthExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result);
}
