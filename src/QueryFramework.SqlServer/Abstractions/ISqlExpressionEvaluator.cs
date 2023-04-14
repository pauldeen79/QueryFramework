namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluator
{
    string GetSqlExpression(Expression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context);
    string GetLengthExpression(Expression expression, IQueryFieldInfo fieldInfo, object? context);
}
