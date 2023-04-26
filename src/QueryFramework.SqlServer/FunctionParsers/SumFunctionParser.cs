namespace QueryFramework.SqlServer.FunctionParsers;

public class SumFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is SumExpression)
        {
            sqlExpression = "SUM({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
