namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluator
{
    string GetSqlExpression(IExpression expression, IQueryFieldInfo fieldInfo, int paramCounter);
    string GetLengthExpression(IExpression expression, IQueryFieldInfo fieldInfo);
}
