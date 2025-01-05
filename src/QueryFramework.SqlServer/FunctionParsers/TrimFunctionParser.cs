namespace QueryFramework.SqlServer.FunctionParsers;

public class TrimFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is TrimExpression trimExpression)
        {
            sqlExpression = "TRIM({0})";
            innerExpression = trimExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
