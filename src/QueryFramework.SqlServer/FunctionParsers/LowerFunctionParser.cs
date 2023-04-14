namespace QueryFramework.SqlServer.FunctionParsers;

public class LowerFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is ToLowerCaseExpression)
        {
            sqlExpression = "LOWER({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
