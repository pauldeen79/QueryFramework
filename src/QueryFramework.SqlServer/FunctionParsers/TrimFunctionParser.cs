namespace QueryFramework.SqlServer.FunctionParsers;

public class TrimFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is TrimExpression)
        {
            sqlExpression = "TRIM({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
