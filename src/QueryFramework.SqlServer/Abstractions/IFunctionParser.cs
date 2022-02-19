namespace QueryFramework.SqlServer.Abstractions;

public interface IFunctionParser
{
    bool TryParse(IExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression);
}
