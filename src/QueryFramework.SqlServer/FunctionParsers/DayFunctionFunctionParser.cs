namespace QueryFramework.SqlServer.FunctionParsers;

public class DayFunctionFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is DayExpression dayExpression)
        {
            sqlExpression = "DAY({0})";
            innerExpression = dayExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
