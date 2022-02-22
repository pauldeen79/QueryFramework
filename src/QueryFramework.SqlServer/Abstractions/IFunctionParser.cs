namespace QueryFramework.SqlServer.Abstractions;

public interface IFunctionParser
{
    bool TryParse(IExpressionFunction function, ISqlExpressionEvaluator evaluator, out string sqlExpression);
}
