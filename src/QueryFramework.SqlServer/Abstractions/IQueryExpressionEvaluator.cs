namespace QueryFramework.SqlServer.Abstractions;

public interface IQueryExpressionEvaluator
{
    string GetSqlExpression(IExpression expression);
}
