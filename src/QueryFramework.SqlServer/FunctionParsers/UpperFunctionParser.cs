namespace QueryFramework.SqlServer.FunctionParsers;

public class UpperFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is ToUpperCaseExpression)
        {
            sqlExpression = "UPPER({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
