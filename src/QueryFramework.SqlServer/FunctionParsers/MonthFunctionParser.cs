namespace QueryFramework.SqlServer.FunctionParsers;

public class MonthFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is MonthExpression)
        {
            sqlExpression = "MONTH({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
