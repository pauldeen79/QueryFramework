namespace QueryFramework.SqlServer.FunctionParsers;

public class CountFunctionFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is CountExpression)
        {
            sqlExpression = "COUNT({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
