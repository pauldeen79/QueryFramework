namespace QueryFramework.SqlServer.Abstractions;

public interface IFunctionParser
{
    bool TryParse(IQueryExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression);
}
