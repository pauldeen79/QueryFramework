namespace QueryFramework.SqlServer.FunctionParsers;

public class LengthFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression, out Expression innerExpression)
    {
        if (expression is StringLengthExpression stringLengthExpression)
        {
            sqlExpression = "LEN({0})";
            innerExpression = stringLengthExpression.Expression.ToUntyped();
            return true;
        }

        sqlExpression = string.Empty;
        innerExpression = new EmptyExpression();
        return false;
    }
}
