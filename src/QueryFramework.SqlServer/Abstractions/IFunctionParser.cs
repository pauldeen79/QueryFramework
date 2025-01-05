namespace QueryFramework.SqlServer.Abstractions;

public interface IFunctionParser
{
    bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression);
}
