namespace QueryFramework.SqlServer.FunctionParsers;

public class MonthFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is MonthExpression monthExpression)
        {
            sqlExpression = "MONTH({0})";
            innerExpression = monthExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
