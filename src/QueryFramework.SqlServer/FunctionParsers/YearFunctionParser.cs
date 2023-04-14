namespace QueryFramework.SqlServer.FunctionParsers;

public class YearFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is YearExpression)
        {
            sqlExpression = "YEAR({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
