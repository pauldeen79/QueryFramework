namespace QueryFramework.SqlServer.Abstractions;

public interface ISqlExpressionEvaluator
{
    string GetSqlExpression(IExpression expression);
}
