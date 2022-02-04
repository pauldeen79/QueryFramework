namespace QueryFramework.SqlServer.Abstractions;

public interface IQueryExpressionEvaluator
{
    string GetSqlExpression(IQueryExpression expression);
}
