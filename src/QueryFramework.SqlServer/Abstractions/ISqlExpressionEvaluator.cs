namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluator
{
    string GetSqlExpression(IExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag);
    string GetLengthExpression(IExpression expression, IQueryFieldInfo fieldInfo);
}
