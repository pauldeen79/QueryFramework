namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluator
{
    string GetSqlExpression(IQuery query, Expression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag);
    string GetLengthExpression(IQuery query, Expression expression, IQueryFieldInfo fieldInfo);
}
