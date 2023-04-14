namespace QueryFramework.SqlServer.FunctionParsers;

public class LengthFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is StringLengthExpression)
        {
            sqlExpression = "LEN({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
