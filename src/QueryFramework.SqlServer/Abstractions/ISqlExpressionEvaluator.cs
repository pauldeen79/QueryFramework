namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluator
{
    string GetSqlExpression(IQuery query, Expression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context);
    string GetLengthExpression(IQuery query, Expression expression, IQueryFieldInfo fieldInfo, object? context);
}
